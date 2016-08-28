/* ====================================================== 
-- NewSpring Script #777: 
-- Creates the metric 2.0 structure 
  
--  Assumptions:
--  Existing metrics structure exists according to script 7:

	TODO:
		1) Fix issues with Fuse, KidSpring, Next Steps naming
		2) Add Schedule id to new metrics

   ====================================================== */
-- Make sure you're using the right Rock database:

--USE Rock

-- Set common variables 
DECLARE @IsSystem bit = 0
DECLARE @True bit = 1
DECLARE @False bit = 0
DECLARE @Order int = 0
DECLARE @MetricSourceSQLId int = (SELECT [Id] FROM DefinedValue WHERE [Guid] = '6A1E1A1B-A636-4E12-B90C-D7FD1BDAE764');
DECLARE @CreatedDateTime AS DATETIME = GETDATE();
DECLARE @foreignKey AS NVARCHAR(15) = 'Metrics 2.0';
DECLARE @metricValueType AS INT = 0;
DECLARE @dvTrue AS INT = (SELECT dv.Id FROM DefinedValue dv JOIN DefinedType dt ON dt.Id = dv.DefinedTypeId WHERE dv.Value = 'True' AND dt.Name = 'Boolean');
DECLARE @dvFalse AS INT = (SELECT dv.Id FROM DefinedValue dv JOIN DefinedType dt ON dt.Id = dv.DefinedTypeId WHERE dv.Value = 'False' AND dt.Name = 'Boolean');

-- Schedule ids variables
DECLARE @service0915 AS INT = (SELECT Id FROM Schedule WHERE Name = 'Sunday 09:15am');;
DECLARE @service1115 AS INT = (SELECT Id FROM Schedule WHERE Name = 'Sunday 11:15am');;
DECLARE @service1600 AS INT = (SELECT Id FROM Schedule WHERE Name = 'Sunday 04:00pm');;
DECLARE @service1800 AS INT = (SELECT Id FROM Schedule WHERE Name = 'Sunday 06:00pm');
DECLARE @sundayMetricSchedule AS INT = (SELECT Id FROM Schedule WHERE Name = 'Sunday Metric Schedule');
DECLARE @fuseMetricSchedule AS INT = (SELECT Id FROM Schedule WHERE Name = 'Fuse Metric Schedule');

-- Structure ids
DECLARE @etidMetricCategory AS INT = 189;
DECLARE @volunteerMetricCategoryId AS INT = (SELECT Id FROM Category WHERE EntityTypeId = @etidMetricCategory AND Name = 'Volunteers');

-- Debug variables
DECLARE @metricId AS INT = 912;

-- Source SQL
DECLARE @sourceAttendance AS NVARCHAR(MAX) = N'
DECLARE @BooleanDTId INT = (SELECT ID FROM DefinedType WHERE Name = ''Boolean'')
DECLARE @TrueDVId INT = (SELECT ID FROM DefinedValue WHERE Value = ''True'' AND DefinedTypeId = @BooleanDTId)
DECLARE @FalseDVId INT = (SELECT ID FROM DefinedValue WHERE Value = ''False'' AND DefinedTypeId = @BooleanDTId)
DECLARE @SchedulesDTId INT = (SELECT ID FROM DefinedType WHERE Name = ''Schedules'')
DECLARE @GroupMemberETId INT = (SELECT ID FROM EntityType WHERE Name = ''Rock.Model.GroupMember'')	
DECLARE @GroupTypeId INT = {{ Metric.MetricPartitions | Where:''Label'', ''Group'' | Select:''EntityTypeQualifierValue'' }}	
DECLARE @Today AS DATE = GETDATE()
DECLARE @RecentSundayDate AS DATE = CONVERT(DATE, DATEADD(DAY, 1 - DATEPART(DW, @Today), @Today));

;WITH GroupAttendance as (
	SELECT COUNT(1) Value, 
		CONVERT(DATE, StartDateTime) MetricValueDateTime, 
		ISNULL(a.CampusId, g.CampusId) CampusId, 
		GroupId, 
		a.ScheduleId, 
		@TrueDVId DidAttend
	FROM Attendance a
	INNER JOIN [Group] g
		ON a.GroupId = g.Id
		AND g.GroupTypeId = @GroupTypeId
		AND a.DidAttend = 1	
	WHERE StartDateTime >= @RecentSundayDate 
		AND StartDateTime < DATEADD(day, 1, @Today)
	GROUP BY 
		CONVERT(DATE, StartDateTime), 
		ISNULL(a.CampusId, g.CampusId), 
		a.GroupId, 
		a.ScheduleId
)
, GroupRoster as (
	SELECT COUNT(1) Value, 
		@Today MetricValueDateTime, 
		g.CampusId, 
		gm.GroupId, 
		ScheduleAssignment.ScheduleId
	FROM GroupMember gm
	INNER JOIN [Group] g
		ON gm.GroupId = g.Id
		and g.GroupTypeId = @GroupTypeId
	INNER JOIN Attribute a
		ON a.EntityTypeId = @GroupMemberETId
		AND a.EntityTypeQualifierColumn = ''GroupTypeId''
		AND a.EntityTypeQualifierValue = @GroupTypeId
		AND a.Name = ''Schedule''
	/* Look for all assignments, not just ones with schedules */
	LEFT JOIN (
		SELECT DISTINCT AV.EntityId, DV.ForeignId ScheduleId
		FROM DefinedValue DV
		LEFT JOIN (
			SELECT EntityId, r.value(''.'', ''UNIQUEIDENTIFIER'') AS ScheduleGuid
			FROM (
			    /* Denormalize the comma-delimited GUID string */
				SELECT EntityId, CAST(''<n>'' + REPLACE(Value, '','', ''</n><n>'') + ''</n>'' AS XML) AS Schedules
				FROM [Attribute] SA
				INNER JOIN AttributeValue AV
					ON AV.AttributeId = SA.Id
					AND SA.[Key] = ''Schedule''
					AND SA.EntityTypeQualifierColumn = ''GroupTypeId''
					AND SA.EntityTypeQualifierValue = @GroupTypeId
					AND AV.Value <> ''''
			) AS nodes 
			/* Parse the xml as a table for joining */
			CROSS APPLY Schedules.nodes(''n'') AS parse(r)
		) AV 
			ON AV.ScheduleGuid = DV.[Guid]
		WHERE AV.EntityId IS NOT NULL
			AND DV.DefinedTypeId = @SchedulesDTId
	) ScheduleAssignment
		ON GM.Id = ScheduleAssignment.EntityId
	GROUP BY g.CampusId, 
		gm.GroupId, 
		ScheduleAssignment.ScheduleId
)
/* Get all the Did Attend values */
SELECT * FROM GroupAttendance 
UNION ALL
/* Get the Did Not Attend values */
SELECT CASE WHEN r.Value - a.Value < 0 THEN 0 ELSE ISNULL(r.Value - a.Value, r.Value) END as Value, 
	ISNULL(a.MetricValueDateTime, @Today) MetricValueDateTime, 
	r.CampusId, 
	r.GroupId, 
	r.ScheduleId, 
	@FalseDVId
FROM GroupRoster r
LEFT JOIN GroupAttendance a
	ON a.CampusId = r.CampusId
	AND a.GroupId = r.GroupId
	AND (r.ScheduleId IS NULL OR r.ScheduleId = a.ScheduleId)
';

DECLARE @sourceUnique AS NVARCHAR(MAX) = N'
DECLARE @BooleanDTId INT = (SELECT ID FROM DefinedType WHERE Name = ''Boolean'')
DECLARE @TrueDVId INT = (SELECT ID FROM DefinedValue WHERE Value = ''True'' AND DefinedTypeId = @BooleanDTId)
DECLARE @GroupTypeId INT = {{ Metric.MetricPartitions | Where:''Label'', ''Group'' | Select:''EntityTypeQualifierValue'' }}
DECLARE @Today AS DATE = GETDATE()
DECLARE @RecentSundayDate AS DATE = CONVERT(DATE, DATEADD(DAY, 1 - DATEPART(DW, @Today), @Today));

SELECT 
	COUNT(1) Value, 
	CONVERT(DATE, StartDateTime) MetricValueDateTime, 
	ISNULL(a.campusid, g.campusid) CampusId, 
	GroupId, 
	@TrueDVId DidAttend
FROM 
	Attendance a
	INNER JOIN [Group] g 
		ON a.GroupId = g.Id
		AND g.GroupTypeId = @GroupTypeId
		AND a.DidAttend = 1	
WHERE 
	StartDateTime >= @RecentSundayDate 
	AND StartDateTime < DATEADD(day, 1, @Today)
GROUP BY 
	CONVERT(DATE, StartDateTime),
	ISNULL(a.CampusId, g.CampusId), 
	a.GroupId		
ORDER BY
	GroupId,
	CONVERT(DATE, StartDateTime),
	ISNULL(a.CampusId, g.CampusId)
';

/* ====================================================== */
-- create the group conversion table
/* ====================================================== */
IF object_id('tempdb..#groupConversion') IS NOT NULL
BEGIN
	drop table #groupConversion
END

select ogt.Name OldGroupType, og.id OldGroupId, og.name OldGroup, ogl.Id OldLocationId, ogl.name OldLocation, gt.Name GroupTypeName, ng.Id GroupId, ng.Name GroupName, ng.CampusId
into #groupConversion
from [group] og
	inner join grouptype ogt
	on og.grouptypeid = ogt.id
	and ogt.name not like 'NEW %'    
	inner join grouplocation gl
	on og.id = gl.groupid
	and og.isactive = 1    
	inner join location ogl
	on gl.LocationId = ogl.id
	and ogl.name is not null    
	inner join [group] ng
	on ng.name = ogl.name
	inner join grouptype gt
	on ng.grouptypeid = gt.id
	and gt.name like 'NEW %'
	inner join grouplocation ngl
	on ng.id = ngl.groupid
	and ogl.id = ngl.LocationId;

/* ====================================================== */
-- create the initial metric structure
/* ====================================================== */
IF object_id('tempdb..#metricTypes') IS NOT NULL
BEGIN
	drop table #metricTypes
END

SELECT 
	pc.Name AS CategoryName,
	pc.Id AS CategoryId,
	NEWID() AS AttendanceMetricGuid,
	NEWID() AS UniqueMetricGuid,
	g.GroupTypeId,
	ngt.Id AS NewGroupTypeId,
	CASE WHEN gt.Name LIKE '%Attendee' THEN 'Attendee' ELSE 'Volunteer' END AS GroupTypeName
INTO #metricTypes
FROM 
	Metric m
	JOIN MetricCategory mc ON mc.MetricId = m.Id
	JOIN [Group] g ON g.Id = m.ForeignId
	JOIN Category c ON c.Id = mc.CategoryId
	JOIN Category pc ON pc.Id = c.ParentCategoryId
	JOIN GroupType gt ON g.GroupTypeId = gt.Id
	JOIN GroupType ngt ON ngt.Name = CONCAT('NEW ', gt.Name)
WHERE 
	Title LIKE '% Service Attendance'
	AND pc.ParentCategoryId = @volunteerMetricCategoryId
GROUP BY
	pc.Name,
	pc.Id,
	g.GroupTypeId,
	ngt.Id,
	gt.Name;

/* ====================================================== */
-- insert the new metrics
/* ====================================================== */
-- Attendance
INSERT INTO Metric (
	IsSystem,
	Title,
	[Description],
	IsCumulative,
	SourceValueTypeId,
	SourceSql,
	CreatedDateTime,
	[Guid],
	ForeignKey,
	IconCssClass,
	ScheduleId
)
SELECT
	@IsSystem AS IsSystem,
	CONCAT(mt.GroupTypeName, ' Group Attendance') AS [Title],
	'Metric to track attendance' AS [Description],
	0 AS IsCumulative,
	@MetricSourceSQLId AS SourceValueTypeId,
	@sourceAttendance AS SourceSql,
	@CreatedDateTime AS CreatedDateTime,
	mt.AttendanceMetricGuid AS [Guid],
	@foreignKey AS ForeignKey,
	'' AS IconCssClass,
	@sundayMetricSchedule
FROM
	#metricTypes mt;

-- Unique
INSERT INTO Metric (
	IsSystem,
	Title,
	[Description],
	IsCumulative,
	SourceValueTypeId,
	SourceSql,
	CreatedDateTime,
	[Guid],
	ForeignKey,
	IconCssClass,
	ScheduleId
)
SELECT
	@IsSystem AS IsSystem,
	CONCAT(mt.GroupTypeName, ' Group Uniques') AS [Title],
	'Metric to track unique volunteers' AS [Description],
	0 AS IsCumulative,
	@MetricSourceSQLId AS SourceValueTypeId,
	@sourceUnique AS SourceSql,
	@CreatedDateTime AS CreatedDateTime,
	mt.UniqueMetricGuid AS [Guid],
	@foreignKey AS ForeignKey,
	'' AS IconCssClass,
	@sundayMetricSchedule
FROM
	#metricTypes mt;

/* ====================================================== */
-- add the new metrics to a category
/* ====================================================== */
-- Attendance
INSERT INTO MetricCategory (
	MetricId,
	CategoryId,
	[Order],
	[Guid],
	ForeignKey
)
SELECT
	(SELECT Id FROM Metric WHERE [Guid] = mt.AttendanceMetricGuid) AS MetricId,
	mt.CategoryId,
	@Order AS [Order],
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM
	#metricTypes mt;

-- Unique
INSERT INTO MetricCategory (
	MetricId,
	CategoryId,
	[Order],
	[Guid],
	ForeignKey
)
SELECT
	(SELECT Id FROM Metric WHERE [Guid] = mt.UniqueMetricGuid) AS MetricId,
	mt.CategoryId,
	@Order AS [Order],
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM
	#metricTypes mt;

/* ====================================================== */
-- add the new metric partitions for attendance
/* ====================================================== */
-- Campus
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.AttendanceMetricGuid) AS MetricId,
	'Campus' AS Label,
	67 AS EntityTypeId,
	@True AS IsRequired,
	0 AS [Order],
	'' AS EntityTypeQualifierColumn,
	'' AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

-- Group
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.AttendanceMetricGuid) AS MetricId,
	'Group' AS Label,
	16 AS EntityTypeId,
	@False AS IsRequired,
	1 AS [Order],
	'GroupTypeId' AS EntityTypeQualifierColumn,
	mt.NewGroupTypeId AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

-- Schedule
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.AttendanceMetricGuid) AS MetricId,
	'Schedule' AS Label,
	54 AS EntityTypeId,
	@False AS IsRequired,
	2 AS [Order],
	'' AS EntityTypeQualifierColumn,
	'' AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

-- Did attend
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.AttendanceMetricGuid) AS MetricId,
	'Did Attend' AS Label,
	31 AS EntityTypeId,
	@False AS IsRequired,
	3 AS [Order],
	'DefinedTypeId' AS EntityTypeQualifierColumn,
	'72' AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

/* ====================================================== */
-- add the new metric partitions for unique
/* ====================================================== */
-- Campus
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.UniqueMetricGuid) AS MetricId,
	'Campus' AS Label,
	67 AS EntityTypeId,
	@True AS IsRequired,
	0 AS [Order],
	'' AS EntityTypeQualifierColumn,
	'' AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

-- Group
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.UniqueMetricGuid) AS MetricId,
	'Group' AS Label,
	16 AS EntityTypeId,
	@False AS IsRequired,
	1 AS [Order],
	'GroupTypeId' AS EntityTypeQualifierColumn,
	mt.NewGroupTypeId AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

-- Did attend
INSERT INTO MetricPartition(
	MetricId,
	Label,
	EntityTypeId,
	IsRequired,
	[Order],
	EntityTypeQualifierColumn,
	EntityTypeQualifierValue,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT Id FROM Metric WHERE [Guid] = mt.UniqueMetricGuid) AS MetricId,
	'Did Attend' AS Label,
	31 AS EntityTypeId,
	@False AS IsRequired,
	2 AS [Order],
	'DefinedTypeId' AS EntityTypeQualifierColumn,
	'72' AS EntityTypeQualifierValue,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#metricTypes mt;

/* ====================================================== */
-- convert the metric values for service attendance to attendance
/* ====================================================== */
IF object_id('tempdb..#attendanceMetricValues') IS NOT NULL
BEGIN
	drop table #attendanceMetricValues
END

SELECT 
	m.Id AS MetricId,
	mv.YValue,
	CONVERT(DATE, mv.MetricValueDateTime) AS MetricDate,
	CASE 
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '09' THEN @service0915
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '11' THEN @service1115 
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '16' THEN @service1600 
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '18' THEN @service1800
	END AS ScheduleId,
	1 AS DidAttend,
	c.Id AS CampusId,
	ng.GroupId AS NewGroupId,
	NEWID() AS MetricValueGuid,
	mt.AttendanceMetricGuid
INTO #attendanceMetricValues
FROM 
	Metric m
	JOIN MetricCategory mc ON mc.MetricId = m.Id
	JOIN #metricTypes mt ON mt.CategoryId = mc.CategoryId
	JOIN [Group] g ON g.Id = m.ForeignId
	JOIN MetricPartition mp ON mp.MetricId = m.Id
	JOIN EntityType et ON et.Id = mp.EntityTypeId
	JOIN #groupConversion ng ON ng.OldGroupId = g.Id
	JOIN MetricValue mv ON mv.MetricId = m.Id
	JOIN MetricValuePartition mvp ON mvp.MetricPartitionId = mp.Id AND mvp.MetricValueId = mv.Id AND ng.CampusId = mvp.EntityId
	JOIN Campus c ON c.Id = ng.CampusId
WHERE 
	m.Title LIKE '% Service Attendance';

-- Add metric values
INSERT INTO MetricValue(
	MetricValueType,
	YValue,
	MetricId,
	MetricValueDateTime,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	@metricValueType,
	amv.YValue,
	(SELECT Id FROM Metric WHERE [Guid] = amv.AttendanceMetricGuid) AS MetricId,
	amv.MetricDate,
	@CreatedDateTime,
	amv.MetricValueGuid,
	@foreignKey
FROM 
	#attendanceMetricValues amv;

/* ====================================================== */
-- add partition values
/* ====================================================== */
-- Add partition values for campus
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Campus') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.CampusId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#attendanceMetricValues amv;

-- Add partition values for group
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Group') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.NewGroupId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#attendanceMetricValues amv;

-- Add partition values for service
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Schedule') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.ScheduleId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#attendanceMetricValues amv;

-- Add partition values for did attend
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Did Attend') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	@dvTrue AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#attendanceMetricValues amv;

/* ====================================================== */
-- calculate did not attend values
/* ====================================================== */
IF object_id('tempdb..#rosterMetricValues') IS NOT NULL
BEGIN
	drop table #rosterMetricValues
END

SELECT 
	m.Id AS MetricId,
	mv.YValue,
	CONVERT(DATE, mv.MetricValueDateTime) AS MetricDate,
	CASE 
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '09' THEN @service0915
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '11' THEN @service1115 
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '16' THEN @service1600 
		WHEN SUBSTRING(CONVERT(NVARCHAR(30), CONVERT(TIME, mv.MetricValueDateTime)), 0, 3) = '18' THEN @service1800
	END AS ScheduleId,
	1 AS DidAttend,
	c.Id AS CampusId,
	ng.GroupId AS NewGroupId,
	mt.AttendanceMetricGuid,
	NEWID() AS MetricValueGuid
INTO #rosterMetricValues
FROM 
	Metric m
	JOIN MetricCategory mc ON mc.MetricId = m.Id
	JOIN #metricTypes mt ON mt.CategoryId = mc.CategoryId
	JOIN [Group] g ON g.Id = m.ForeignId
	JOIN MetricPartition mp ON mp.MetricId = m.Id
	JOIN EntityType et ON et.Id = mp.EntityTypeId
	JOIN #groupConversion ng ON ng.OldGroupId = g.Id
	JOIN MetricValue mv ON mv.MetricId = m.Id
	JOIN MetricValuePartition mvp ON mvp.MetricPartitionId = mp.Id AND mvp.MetricValueId = mv.Id AND ng.CampusId = mvp.EntityId
	JOIN Campus c ON c.Id = ng.CampusId
WHERE 
	m.Title LIKE '% Service Roster';

-- Subtract attendance to get did not attend count
UPDATE rmv
SET rmv.YValue = rmv.YValue - amv.YValue
FROM
	#attendanceMetricValues amv
	JOIN #rosterMetricValues rmv ON 
		rmv.CampusId = amv.CampusId
		AND rmv.MetricDate = amv.MetricDate
		AND rmv.NewGroupId = amv.NewGroupId
		AND rmv.ScheduleId = amv.ScheduleId

-- XXX Fix negative values
UPDATE rmv
SET rmv.YValue = 0
FROM #rosterMetricValues rmv
WHERE rmv.YValue < 0

-- Add metric values
INSERT INTO MetricValue(
	MetricValueType,
	YValue,
	MetricId,
	MetricValueDateTime,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	@metricValueType,
	amv.YValue,
	(SELECT Id FROM Metric WHERE [Guid] = amv.AttendanceMetricGuid) AS MetricId,
	amv.MetricDate,
	@CreatedDateTime,
	amv.MetricValueGuid,
	@foreignKey
FROM 
	#rosterMetricValues amv;

/* ====================================================== */
-- add partition values
/* ====================================================== */
-- Add partition values for campus
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Campus') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.CampusId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#rosterMetricValues amv;

-- Add partition values for group
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Group') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.NewGroupId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#rosterMetricValues amv;

-- Add partition values for service
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Schedule') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.ScheduleId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#rosterMetricValues amv;

-- Add partition values for did attend
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.AttendanceMetricGuid AND mp.Label = 'Did Attend') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	@dvFalse AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#rosterMetricValues amv;

/* ====================================================== */
-- convert the metric values for service attendance to attendance
/* ====================================================== */
IF object_id('tempdb..#uniqueMetricValues') IS NOT NULL
BEGIN
	drop table #uniqueMetricValues
END

SELECT 
	m.Id AS MetricId,
	mv.YValue,
	CONVERT(DATE, mv.MetricValueDateTime) AS MetricDate,
	1 AS DidAttend,
	c.Id AS CampusId,
	ng.GroupId AS NewGroupId,
	NEWID() AS MetricValueGuid,
	mt.UniqueMetricGuid
INTO #uniqueMetricValues
FROM 
	Metric m
	JOIN MetricCategory mc ON mc.MetricId = m.Id
	JOIN #metricTypes mt ON mt.CategoryId = mc.CategoryId
	JOIN [Group] g ON g.Id = m.ForeignId
	JOIN MetricPartition mp ON mp.MetricId = m.Id
	JOIN EntityType et ON et.Id = mp.EntityTypeId
	JOIN #groupConversion ng ON ng.OldGroupId = g.Id
	JOIN MetricValue mv ON mv.MetricId = m.Id
	JOIN MetricValuePartition mvp ON mvp.MetricPartitionId = mp.Id AND mvp.MetricValueId = mv.Id AND ng.CampusId = mvp.EntityId
	JOIN Campus c ON c.Id = ng.CampusId
WHERE 
	m.Title LIKE '% Unique Serving';

-- Add metric values
INSERT INTO MetricValue(
	MetricValueType,
	YValue,
	MetricId,
	MetricValueDateTime,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	@metricValueType,
	amv.YValue,
	(SELECT Id FROM Metric WHERE [Guid] = amv.UniqueMetricGuid) AS MetricId,
	amv.MetricDate,
	@CreatedDateTime,
	amv.MetricValueGuid,
	@foreignKey
FROM 
	#uniqueMetricValues amv;

/* ====================================================== */
-- add partition values for unique
/* ====================================================== */
-- Add partition values for campus
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.UniqueMetricGuid AND mp.Label = 'Campus') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.CampusId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#uniqueMetricValues amv;

-- Add partition values for group
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.UniqueMetricGuid AND mp.Label = 'Group') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	amv.NewGroupId AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#uniqueMetricValues amv;

-- Add partition values for did attend
INSERT INTO MetricValuePartition(
	MetricPartitionId,
	MetricValueId,
	EntityId,
	CreatedDateTime,
	[Guid],
	ForeignKey
)
SELECT 
	(SELECT mp.Id FROM MetricPartition mp JOIN Metric m ON m.Id = mp.MetricId WHERE m.[Guid] = amv.UniqueMetricGuid AND mp.Label = 'Did Attend') AS MetricPartitionId,
	(SELECT Id FROM MetricValue WHERE [Guid] = amv.MetricValueGuid) AS MetricValueId,
	@dvTrue AS EntityId,
	@CreatedDateTime AS CreatedDateTime,
	NEWID() AS [Guid],
	@foreignKey AS ForeignKey
FROM 
	#uniqueMetricValues amv;


--DELETE FROM Metric WHERE ForeignKey = 'Metrics 2.0'
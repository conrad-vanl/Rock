/****** Script for SelectTopNRows command from SSMS  ******/
SELECT
	CONCAT(
		'INSERT INTO Attendance (GroupId, StartDateTime, DidAttend, [Guid], CreatedDateTime, ForeignKey, PersonAliasId) VALUES (',
			'(SELECT Id FROM [Group] WHERE ForeignId = ',
				ga.GroupId,
			'), ''',
			CONVERT(DATE, ga.StartDateTime),
			''', 1, NEWID(), GETDATE(), ''F1.GroupsAttendance'', ',
			'(SELECT Id FROM [PersonAlias] WHERE ForeignId = ',
				ga.IndividualID,
			')',
		');',
		CHAR(13),
		CHAR(10),
		'GO',
		CHAR(13),
		CHAR(10)
	)
FROM 
	[GroupsAttendance] ga
WHERE 
	ga.IndividualIsPresent <> 0
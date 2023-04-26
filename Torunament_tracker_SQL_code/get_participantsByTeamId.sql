CREATE DEFINER=`root`@`localhost` PROCEDURE `get_participantsByTeamId`(
	team_id tinyint
)
BEGIN
select 
		participant_id as Id,
    first_name as Firstname,
    last_name as LastName,
    email as EmailAdress,
    phone as CellPhoneNumber
from participants
where participant_id in 
(
select participant_id
from team_members as tm
where tm.team_id = team_id
);
END
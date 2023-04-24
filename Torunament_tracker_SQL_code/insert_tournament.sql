CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_team_member`(
	team_id tinyint,
    participant_id int
)
begin
	insert into team_members(team_id,participant_id)
    values(team_id,participant_id);

end
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_teams_all`()
begin
	select
		team_id as Id,
        team_name as TeamName
	from teams;
end
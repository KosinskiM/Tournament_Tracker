CREATE DEFINER=`root`@`localhost` PROCEDURE `get_teamsByTournamentId`(
	tournament_id tinyint
)
BEGIN
	select 
			team_id as Id,
			team_name as TeamName
	from teams
	where team_id in (
	select team_id
	from tournament_entries as te
	where te.tournament_id = tournament_id
	);
END
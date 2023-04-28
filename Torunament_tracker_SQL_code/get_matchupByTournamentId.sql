CREATE DEFINER=`root`@`localhost` PROCEDURE `get_matchupByTournamentId`(
	tournament_id tinyint
)
BEGIN
select
	matchup_id as Id,
    matchup_round as MatchupRound,
    winner_team_id as WinnerId
from matchups as m
where m.tournament_id = tournament_id;
END
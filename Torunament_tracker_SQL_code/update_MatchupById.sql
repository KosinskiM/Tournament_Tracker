CREATE DEFINER=`root`@`localhost` PROCEDURE `update_MatchupById`(
matchup_id tinyint,
winner_id tinyint
)
BEGIN
update matchups as m
set winner_team_id = winner_id
where m.matchup_id = matchup_id;
END
CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_matchup`(
	tournament_id tinyint,
	matchup_round tinyint,
    out matchup_id int
)
begin
	insert into matchups(tournament_id, matchup_round)
    values(tournament_id, matchup_round);
    
    select max(m.matchup_id)
    into matchup_id
    from matchups as m;
end
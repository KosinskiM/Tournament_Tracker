CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_tournament_entry`(	
	out tournament_entry_id tinyint,
    tournament_id tinyint,
    team_id int
)
begin
	insert into tournament_entries(tournament_id,team_id)
    values(tournament_id, team_id);
    
    select max(te.tournament_entry_id)
    into tournament_entry_id
    from tournament_entries as te;
    
end
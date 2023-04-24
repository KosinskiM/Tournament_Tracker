CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_tournamententry`(	
	out tournament_entry_id tinyint,
    tournament_id tinyint,
    team_id tinyint
)
begin
	insert into tournamententires(tournament_id,team_id)
    values(tournament_id, team_id);
    
    select tournament_entry_id
    from tournament_entries
    where tournament_entry_id = (select count(*) from tournamnet_entries);
    
end
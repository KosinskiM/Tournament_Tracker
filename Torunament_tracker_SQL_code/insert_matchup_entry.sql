CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_matchup_entry`(
	matchup_id tinyint,
    parent_matchup_id tinyint,
    team_competing_id tinyint,
    out matchup_entry_id int
)
begin
	insert into matchup_entries(matchup_id,parent_matchup_id,team_competing_id)
    values(matchup_id,parent_matchup_id,team_competing_id);
    
    select count(*)
    into matchup_entry_id
    from matchup_entries;
end
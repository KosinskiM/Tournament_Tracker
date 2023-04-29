CREATE DEFINER=`root`@`localhost` PROCEDURE `update_entryById`(
	entry_id tinyint,
    score int
)
BEGIN
update matchup_entries
set score = score
where matchup_entry_id = entry_id;
END
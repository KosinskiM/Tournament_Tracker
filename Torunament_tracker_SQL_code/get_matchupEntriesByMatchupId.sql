CREATE DEFINER=`root`@`localhost` PROCEDURE `get_matchupEntriesByMatchupId`(
	matchup_id tinyint
)
BEGIN
select
	matchup_entry_id as Id,
    score as Score,
    parent_matchup_id as ParentMatchupId,
    team_competing_id as TeamCompetingId
from matchup_entries as me
where me.matchup_id = matchup_id;
END
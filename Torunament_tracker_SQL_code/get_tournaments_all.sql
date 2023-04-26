CREATE DEFINER=`root`@`localhost` PROCEDURE `get_tournaments_all`()
BEGIN
	select
		tournament_id as Id,
        tournament_name as TournamentName,
        entry_fee as EntryFee
    from tournaments;
END
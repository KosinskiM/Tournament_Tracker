CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_tournament_prize`(
	prize_id tinyint,
    tournament_id tinyint
)
BEGIN
	insert into tournament_prizes(prize_id,tournament_id)
    values(prize_id, tournament_id);
END
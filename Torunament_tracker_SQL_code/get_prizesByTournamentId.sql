CREATE DEFINER=`root`@`localhost` PROCEDURE `get_prizesByTournamentId`(
	tournament_id tinyint
)
BEGIN
select 
	prize_id as Id,
    place_number as PlaceNumber,
    place_name as PlaceName,
    prize_amount as PrizeAmount,
    prize_percentage as PrizePercentage
from prizes
where prize_id in 
(
select prize_id
from tournament_prizes
);
END
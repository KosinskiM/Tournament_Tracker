CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_prize`(
	place_number 		tinyint,
    place_name 			varchar(50),
    prize_amount 		decimal(10,0),
    prize_percentage 	decimal(2,2),
    out prize_id tinyint
)
begin
	insert into prizes (place_number,place_name,prize_amount,prize_percentage)
    values(
		place_number,
        place_name,
        prize_amount,
        prize_percentage);
        
	select prize_id
    from prizes
    where prize_id = (select count(*) from prizes);
end
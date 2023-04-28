CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_prize`(
	place_number 		tinyint,
    place_name 			varchar(50),
    prize_amount 		decimal(10,0),
    prize_percentage 	decimal(10,2),
    out prize_id int
)
begin
	insert into prizes (place_number,place_name,prize_amount,prize_percentage)
    values(
		place_number,
        place_name,
        prize_amount,
        prize_percentage);
        
    select max(p.prize_id)
    into prize_id
    from prizes p;
end
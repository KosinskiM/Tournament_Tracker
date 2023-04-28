CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_tournament`(
	tournament_name varchar(255),
    entry_fee decimal(10,2),
    out tournament_id int
)
begin
	insert into tournaments(tournament_name,entry_fee)
    values(tournament_name, entry_fee);
    
    select max(t.tournament_id)
    into tournament_id
    from tournaments t;
end
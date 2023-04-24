CREATE DEFINER=`root`@`localhost` PROCEDURE `get_prizes_all`()
begin
	select * 
    from prizes;
end
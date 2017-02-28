-- История баланса пользователя
if exists(select 1 from sysobjects where name = N'UserBalanceHistoryView' and xtype='P') drop proc UserBalanceHistoryView
go
create proc UserBalanceHistoryView (
	@UserId int
	) as
begin
	select h.Operation, h.Balance, h.ReportDate, h.Comment 
	from UserBalanceHistory h 
	where h.UserId = @UserId 
	order by h.ReportDate desc;
end
go
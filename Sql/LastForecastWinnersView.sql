-- Cписок прогнозов, в которых пользователь принял участие
if exists(select 1 from sysobjects where name = N'LastForecastWinnersView' and xtype='P') drop proc LastForecastWinnersView
go
create proc LastForecastWinnersView as
begin
					
	select top 4 u.Nic as 'WinnerName', u.Avatar as 'WinnerAvater', u.Rang as 'WinnerRang', f.*
	from Forecasts f left join Users u on u.Id = f.Winner
	where f.IsClosed = 1
	order by f.ForecastDate desc
				
end
go
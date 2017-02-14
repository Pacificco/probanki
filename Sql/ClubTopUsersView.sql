-- ¬озвращает топ пользователей - членов клуба
if exists(select 1 from sysobjects where name = N'ClubTopUsersView' and xtype='P') drop proc ClubTopUsersView
go
create proc ClubTopUsersView (@UsersCount int) as	
begin	
	begin try 
	
		-- ѕользователи и количество их участий в прогнозах
		declare @UserForecastCount table (	UserId			int		not null,
											ForecastCount	int		not null											
											primary key (UserId)
										)
		insert into @UserForecastCount(UserId,ForecastCount)
		select fu.UserId, COUNT(fu.Id)
		from ForecastsUsers fu		
		group by fu.UserId
		
		-- ѕользователи и количество их побед в прогнозах
		declare @UserForecastWinCount table (	UserId			int		not null,												
												WinCount		int		not null
												primary key (UserId)
											)
		insert into @UserForecastWinCount(UserId,WinCount)
		select u.Id, COUNT(f.Id)
		from Forecasts f join Users u on f.Winner = u.Id
		group by u.Id
		
										
		select top 15 u.Id, u.Nic, u.Avatar, f.ForecastCount, w.WinCount
		from Users u 
			left join @UserForecastCount f on u.Id = f.UserId
			left join @UserForecastWinCount w on u.Id = w.UserId
		where u.IsActive = 1 and u.IsBan = 0 and u.EmailConfirmed = 1 and u.Rols = 'club_member'
		order by w.WinCount desc, f.ForecastCount desc, u.RegistrationDate
	
		return 0
	end try 
	begin catch 
		return 1
	end catch 
end
go
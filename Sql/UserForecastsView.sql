-- C����� ���������, � ������� ������������ ������ �������
if exists(select 1 from sysobjects where name = N'UserForecastsView' and xtype='P') drop proc UserForecastsView
go
create proc UserForecastsView (
	@UserId int,
	@SubjectId int
	) as
begin
					
		if @UserId is null begin
			
			if @SubjectId is null begin
			
				select u.Nic as 'WinnerName', u.Avatar as 'WinnerAvater', u.Rang as 'WinnerRang', null, null, f.*
				from Forecasts f 
					left join Users u on u.Id = f.Winner					
				order by f.ForecastDate desc
			
			end else begin
						
				select u.Nic as 'WinnerName', u.Avatar as 'WinnerAvater', u.Rang as 'WinnerRang', null, null, f.*
				from Forecasts f 
					left join Users u on u.Id = f.Winner
				where f.SubjectId = @SubjectId
				order by f.ForecastDate desc
			
			end
		
		end else begin
		
			if @SubjectId is null begin
			
				select u.Nic as 'WinnerName', u.Avatar as 'WinnerAvater', u.Rang as 'WinnerRang', fu.Value, fu.ReportDate, f.*
				from Forecasts f 
					left join Users u on u.Id = f.Winner and u.Id = @UserId
					left join ForecastsUsers fu on fu.ForecastId = f.Id and fu.UserId = @UserId
				where fu.Value is not null
				order by f.ForecastDate desc
			
			end else begin
						
				select u.Nic as 'WinnerName', u.Avatar as 'WinnerAvater', u.Rang as 'WinnerRang', fu.Value, fu.ReportDate, f.*
				from Forecasts f left join Users u on u.Id = f.Winner and u.Id = @UserId
					left join ForecastsUsers fu on fu.ForecastId = f.Id and fu.UserId = @UserId
				where f.SubjectId = @SubjectId and fu.Value is not null
				order by f.ForecastDate desc
			
			end
		
		end
				
end
go
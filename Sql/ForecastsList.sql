-- ¬озвращает прогнозы дл€ постраничной навигации
if exists(select 1 from sysobjects where name = N'ForecastsList' and xtype='P') drop proc ForecastsList
go
create proc ForecastsList (
	@IsClosed bit, 
	@SubjectId tinyint,
	@RowBegin int,
	@RowEnd int,
	@RowTotalCount int output
) as	
begin	

	create table #tblForecasts(
		RowNum			int				not null
		,Id				int				not null
		,SubjectId		tinyint			not null
		,SubjectName	nvarchar(255)	not null
		,IsClosed		bit				not null
		,Winner			int				NULL
		,WinValue		float			NULL
		,WinAmount		float			not null
		,ForecastDate	datetime		not null
		,CreateDate		datetime		not null		
		,UserCount		int				not null
		primary key (Id) 
	)
	
	if @IsClosed is null begin
		if @SubjectId is null
			insert #tblForecasts(RowNum,Id,SubjectId,SubjectName,IsClosed,Winner,WinValue,WinAmount,ForecastDate,CreateDate,UserCount)			
				select row_number() OVER (order by f.CreateDate desc, f.SubjectId) AS RowNumber, 
					f.Id,f.SubjectId,fs.Name,f.IsClosed,f.Winner,f.WinValue,f.WinAmount,f.ForecastDate,f.CreateDate,0
				from Forecasts f join ForecastSubjects fs on f.SubjectId = fs.Id				
		else
			insert #tblForecasts(RowNum,Id,SubjectId,SubjectName,IsClosed,Winner,WinValue,WinAmount,ForecastDate,CreateDate,UserCount)			
				select row_number() OVER (order by f.CreateDate desc, f.SubjectId) AS RowNumber, 
					f.Id,f.SubjectId,fs.Name,f.IsClosed,f.Winner,f.WinValue,f.WinAmount,f.ForecastDate,f.CreateDate,0
				from Forecasts f join ForecastSubjects fs on f.SubjectId = fs.Id
				where f.SubjectId = @SubjectId
	end else 
	if @IsClosed = 1 begin
		if @SubjectId is null
			insert #tblForecasts(RowNum,Id,SubjectId,SubjectName,IsClosed,Winner,WinValue,WinAmount,ForecastDate,CreateDate,UserCount)			
				select row_number() OVER (order by f.CreateDate desc, f.SubjectId) AS RowNumber, 
					f.Id,f.SubjectId,fs.Name,f.IsClosed,f.Winner,f.WinValue,f.WinAmount,f.ForecastDate,f.CreateDate,0
				from Forecasts f join ForecastSubjects fs on f.SubjectId = fs.Id
				where f.IsClosed = 1
		else
			insert #tblForecasts(RowNum,Id,SubjectId,SubjectName,IsClosed,Winner,WinValue,WinAmount,ForecastDate,CreateDate,UserCount)			
				select row_number() OVER (order by f.CreateDate desc, f.SubjectId) AS RowNumber, 
					f.Id,f.SubjectId,fs.Name,f.IsClosed,f.Winner,f.WinValue,f.WinAmount,f.ForecastDate,f.CreateDate,0
				from Forecasts f join ForecastSubjects fs on f.SubjectId = fs.Id
				where f.IsClosed = 1 and f.SubjectId = @SubjectId
	end else begin
		if @SubjectId is null
			insert #tblForecasts(RowNum,Id,SubjectId,SubjectName,IsClosed,Winner,WinValue,WinAmount,ForecastDate,CreateDate,UserCount)			
				select row_number() OVER (order by f.CreateDate desc, f.SubjectId) AS RowNumber, 
					f.Id,f.SubjectId,fs.Name,f.IsClosed,f.Winner,f.WinValue,f.WinAmount,f.ForecastDate,f.CreateDate,0
				from Forecasts f join ForecastSubjects fs on f.SubjectId = fs.Id
				where f.IsClosed = 0
		else
			insert #tblForecasts(RowNum,Id,SubjectId,SubjectName,IsClosed,Winner,WinValue,WinAmount,ForecastDate,CreateDate,UserCount)			
				select row_number() OVER (order by f.CreateDate desc, f.SubjectId) AS RowNumber, 
					f.Id,f.SubjectId,fs.Name,f.IsClosed,f.Winner,f.WinValue,f.WinAmount,f.ForecastDate,f.CreateDate,0
				from Forecasts f join ForecastSubjects fs on f.SubjectId = fs.Id
				where f.IsClosed = 0 and f.SubjectId = @SubjectId
	end
	
	
	create table #tblForecastsUsers(		
		Id				int		not null				
		,UserCount		int		not null
		primary key (Id) 
	)
	
	insert #tblForecastsUsers
	select f.Id, COUNT(fu.UserId) as userCount
	from #tblForecasts f join ForecastsUsers fu on f.Id = fu.ForecastId
	group by f.Id
	
	update #tblForecasts
	set UserCount = case when fu.UserCount is null then 0 else fu.UserCount end
	from #tblForecasts f left join #tblForecastsUsers fu on f.Id = fu.Id
	
	select * from #tblForecasts where RowNum >= @RowBegin and RowNum <= @RowEnd order by RowNum
	
	select @RowTotalCount = count(1) from #tblForecasts
	
end
go
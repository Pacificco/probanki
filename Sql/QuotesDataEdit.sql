-- Создает запись в таблице графиков
if exists(select 1 from sysobjects where name = N'QuotesDataEdit' and xtype='P') drop proc QuotesDataEdit
go
create proc QuotesDataEdit (@SubjectId int, @ChartValue numeric(18,4), @ChartDate datetime) as	
begin	

	set nocount on

	begin try 

		if exists (select 1 from ChartsData q where q.ChartDate = @ChartDate and q.SubjectId = @SubjectId) begin
		
			update q			
			set q.ChartValue = @ChartValue
			from ChartsData q
			where q.SubjectId = @SubjectId and q.ChartDate = @ChartDate
		
		end else begin
			
			insert into ChartsData(SubjectId, ChartValue, ChartDate)
				values (@SubjectId, @ChartValue, @ChartDate)
					
		end
		
		return 0
	end try 
	begin catch 		
		return 1
	end catch 
	
end
go
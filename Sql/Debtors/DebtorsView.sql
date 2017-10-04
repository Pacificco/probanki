-- -----------------------------------------------------------------------------------
--
-- Процедура возвращает список должников согласно заданным фильтрам
-- 
-- -----------------------------------------------------------------------------------
if exists(select 1 from sysobjects where name = N'DebtorsView' and xtype='P') drop proc DebtorsView
go
create proc DebtorsView (
	@Published				bit,				-- Публикация
	@DebtorId				int,				-- Идентификатор пользователя
	@RegionId				uniqueidentifier,	-- Регион: Все, перечень регионов в всплывающем окне
	@DebtCreatedRange		int,				-- Дата образования долга: 0 - за весь период, 1 - за последний месяц, 2 - за последний год
	@DebtAmountRange		int,				-- Сумма долга: Любая сумма / до 50000 / до 100000 / до 300000 / до 500000 / до 1000000 / > 1000000
	@SalePriceRange			int,				-- Цена продажи: Любая цена / до 50000 / до 100000 / до 300000 / до 500000 / до 1000000 / > 1000000
	@CourtDecisionTypeId	int,				-- Решение суда: Все / Есть / Нет
	@DebtorTypeId			int,				-- Лицо: Все / Банк / МФО / юр.лицо / физ.лицо
	@DebtEssenceTypeId		int,				-- Сущность долга: Любая / банковский кредит / займ МФО / частный долг / прочее
	@OriginalCreditorTypeId	int,				-- Первоначальный кредитор: Любой / Банк / МФО / юр.лицо / физ.лицо
	@DebtSellerTypeId		int,				-- Продавец долга: Любой / Банк / МФО / юр.лицо / физ.лицо
	@RecFrom				int,				-- Номер первой записей, начиная с которой нужно вывести записи
	@RecTo					int,				-- Номер последней записи
	@TotalCount				int out				-- Общее количество записей, найденных по поиску
) as	
begin	
	begin try 
	
		set @TotalCount = 0
				
		-- Результирующая таблица
		declare @tempDebtors table (
			RowNo					int
			,Id						int
			,DebtorTypeId			int
			,Published				bit
			,OriginalCreditorTypeId	int
			,RegionId				uniqueidentifier
			,Locality				nvarchar(255)
			,DebtEssenceTypeId		int
			,CourtDecisionTypeId	int
			,DebtCreatedDate		datetime
			,DebtAmount				money
			,SalePrice				money
			,DebtSellerTypeId		int
			,ContactPerson			nvarchar(255)
			,ContactPhone			int
			,DopPhone				int
			,Email					varchar(129)
			,Comment				nvarchar(2000)
			,CreatedAt				datetime
			,UpdatedAt				datetime			
			primary key (Id)
		)
		
		-- Если задан идентификатор должника
		if @DebtorId is not null begin
		
			insert @tempDebtors
			select 1
					,d.Id
					,d.DebtorTypeId
					,d.Published
					,d.OriginalCreditorTypeId
					,d.RegionId
					,d.Locality
					,d.DebtEssenceTypeId
					,d.CourtDecisionTypeId
					,d.DebtCreatedDate
					,d.DebtAmount
					,d.SalePrice
					,d.DebtSellerTypeId
					,d.ContactPerson
					,d.ContactPhone
					,d.DopPhone
					,d.Email
					,d.Comment
					,d.CreatedAt
					,d.UpdatedAt					
			from Debtors d				
			where d.Id = @DebtorId
			
			select @TotalCount = (select COUNT(1) from @tempDebtors)			
			select * from @tempDebtors as result		
			return 0
			
		end		
				
				
		-- Регионы ------------------------------------
		declare @regions table (
			Id	uniqueidentifier
			primary key (Id)
		)
		if @RegionId is not null begin		
			insert @regions(Id) values(@RegionId)		
		end else begin		
			insert @regions(Id)
			select r.AoId from Regions r		
		end
		-- Регионы ------------------------------------
		
		-- Дата образования долга ------------------------------------		
		declare @debtDateFrom datetime = '19010101 00:00:00'
		declare @debtDateTo datetime = dateadd(day, 1, getdate())
		set @debtDateFrom = 
		case @DebtCreatedRange
			when 1 then dateadd(month, -1, getdate())
			when 2 then dateadd(year, -1, getdate())			
		end
		/*if @DebtCreatedDateFrom is not null and @DebtCreatedDateTo is not null begin
			set @debtDateFrom = @DebtCreatedDateFrom
			set @debtDateTo = @DebtCreatedDateTo
		end*/
		-- Дата образования долга ------------------------------------
		
		-- Сумма долга ------------------------------------
		declare @debtAmountFrom money = 0
		declare @debtAmountTo money = 1000000000
		if @DebtAmountRange is not null begin
			set @debtAmountFrom =
			case @DebtAmountRange
				when 1 then 0
				when 2 then 50000
				when 3 then 100000	
				when 4 then 300000
				when 5 then 500000
				when 6 then 1000000
			end
			set @debtAmountTo =
			case @DebtAmountRange
				when 1 then 50000
				when 2 then 100000	
				when 3 then 300000
				when 4 then 500000
				when 5 then 1000000
				when 6 then 1000000000
			end
		end
		-- Сумма долга ------------------------------------
				
		-- Цена продажи ------------------------------------
		declare @salePriceFrom money = 0
		declare @salePriceTo money = 1000000000
		if @SalePriceRange is not null begin
			set @salePriceFrom =
			case @SalePriceRange
				when 1 then 0
				when 2 then 50000
				when 3 then 100000	
				when 4 then 300000
				when 5 then 500000
				when 6 then 1000000
			end
			set @debtAmountTo =
			case @salePriceTo
				when 1 then 50000
				when 2 then 100000	
				when 3 then 300000
				when 4 then 500000
				when 5 then 1000000
				when 6 then 1000000000
			end
		end
		-- Цена продажи ------------------------------------
		
		-- Решение суда ------------------------------------
		declare @courtDecisions table (
			Id	int
			primary key (Id)
		)
		if @CourtDecisionTypeId is not null begin		
			insert @courtDecisions(Id) values(@CourtDecisionTypeId)		
		end else begin		
			insert @courtDecisions(Id)
			select t.Id from CourtDecisionTypes t		
		end
		-- Решение суда ------------------------------------
		
		-- Лицо ------------------------------------
		declare @debtors table (
			Id	int
			primary key (Id)
		)
		if @DebtorId is not null begin		
			insert @debtors(Id) values(@DebtorId)		
		end else begin		
			insert @debtors(Id)
			select t.Id from DebtorTypes t		
		end
		-- Лицо ------------------------------------
		
		-- Сущность долга ------------------------------------
		declare @debtEssences table (
			Id	int
			primary key (Id)
		)
		if @DebtEssenceTypeId is not null begin		
			insert @debtEssences(Id) values(@DebtEssenceTypeId)		
		end else begin		
			insert @debtEssences(Id)
			select t.Id from DebtEssenceTypes t		
		end
		-- Сущность долга ------------------------------------
		
		-- Первоначальный кредитор ------------------------------------
		declare @originalCreditors table (
			Id	int
			primary key (Id)
		)
		if @OriginalCreditorTypeId is not null begin		
			insert @originalCreditors(Id) values(@DebtEssenceTypeId)		
		end else begin		
			insert @originalCreditors(Id)
			select t.Id from OriginalCreditorTypes t		
		end
		-- Первоначальный кредитор ------------------------------------
		
		-- Продавец долга ------------------------------------
		declare @debtSellers table (
			Id	int
			primary key (Id)
		)
		if @DebtSellerTypeId is not null begin		
			insert @debtSellers(Id) values(@DebtSellerTypeId)		
		end else begin		
			insert @debtSellers(Id)
			select t.Id from DebtSellerTypes t		
		end
		-- Продавец долга ------------------------------------

		
		insert @tempDebtors
		select *  
		from (
			select row_number() over (order by DebtCreatedDate desc) as RowIndex
					,d.Id
					,d.DebtorTypeId
					,d.Published
					,d.OriginalCreditorTypeId
					,d.RegionId
					,d.Locality
					,d.DebtEssenceTypeId
					,d.CourtDecisionTypeId
					,d.DebtCreatedDate
					,d.DebtAmount
					,d.SalePrice
					,d.DebtSellerTypeId
					,d.ContactPerson
					,d.ContactPhone
					,d.DopPhone
					,d.Email
					,d.Comment
					,d.CreatedAt
					,d.UpdatedAt					
			from Debtors d
				join @regions r on d.RegionId = r.Id
				join @courtDecisions cd on d.CourtDecisionTypeId = cd.Id
				join @debtors dt on d.DebtorTypeId = dt.Id
				join @debtEssences e on d.DebtEssenceTypeId = e.Id
				join @originalCreditors oc on d.OriginalCreditorTypeId = oc.Id
				join @debtSellers s on d.DebtSellerTypeId = s.Id
			where d.DebtAmount >= @debtAmountFrom and d.DebtAmount < @debtAmountTo and
				d.SalePrice >= @salePriceFrom and d.SalePrice < @salePriceTo and
				d.Published = @Published
		) as result
		
		select @TotalCount = (select COUNT(1) from @tempDebtors)
		select * from @tempDebtors as result
		where result.RowNo >= @RecFrom and result.RowNo <= @RecTo
	
		return 0
	end try 
	begin catch 
		return 1
	end catch 
end
go
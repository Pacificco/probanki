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
	@DebtorTypeId			int,				-- Лицо: Все / юр.лицо / физ.лицо
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
			RowNo						int
			,Id							int
			,DebtorTypeId				int
			,Published					bit
			,OriginalCreditorTypeId		int
			,RegionId					uniqueidentifier
			,Locality					nvarchar(255)
			,DebtEssenceTypeId			int
			,CourtDecisionTypeId		int
			,DebtCreatedDate			datetime
			,DebtAmount					money
			,SalePrice					money
			,DebtSellerTypeId			int
			,ContactPerson				nvarchar(255)
			,ContactPhone				int
			,DopPhone					int
			,Email						varchar(129)
			,Comment					nvarchar(2000)
			,CreatedAt					datetime
			,UpdatedAt					datetime
			,RegionName					nvarchar(255)
			,DebtorTypeName				nvarchar(255)
			,OriginalCreditorTypeName	nvarchar(255)
			,CourtDecisionTypeName		nvarchar(255)
			,DebtSellerTypeName			nvarchar(255)
			,DebtEssenceTypeName		nvarchar(255)
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
					,r.FormalName as 'RegionName'
					,t.Name + ' ' + r.ShortName as 'DebtorTypeName'
					,o.Name as 'OriginalCreditorTypeName'
					,c.Name as 'CourtDecisionTypeName'
					,s.Name as 'DebtSellerTypeName'
					,e.Name as 'DebtEssenceTypeName'
			from Debtors d
				join Regions r on r.AoGuid = d.RegionId
				join DebtorTypes t on t.Id = d.DebtorTypeId
				join DebtSellerTypes s on s.Id = d.DebtSellerTypeId
				join DebtEssenceTypes e on e.Id = d.DebtEssenceTypeId
				join CourtDecisionTypes c on c.Id = d.CourtDecisionTypeId
				join OriginalCreditorTypes o on o.Id = d.OriginalCreditorTypeId
			where d.Id = @DebtorId
			
			select @TotalCount = (select COUNT(1) from @tempDebtors)			
			select * from @tempDebtors
			return 0
			
		end		
		
		-- Дата образования долга ------------------------------------		
		declare @debtDateFrom datetime = '19010101 00:00:00'
		declare @debtDateTo datetime = dateadd(day, 1, getdate())
		if @DebtCreatedRange is not null and @DebtCreatedRange > 0 begin
			set @debtDateFrom = 
			case @DebtCreatedRange
				when 1 then dateadd(month, -1, getdate())
				when 2 then dateadd(year, -1, getdate())			
			end
		end
		-- Дата образования долга ------------------------------------
		
		-- Сумма долга ------------------------------------
		declare @debtAmountFrom money = 0
		declare @debtAmountTo money = 1000000000
		if @DebtAmountRange is not null and @DebtAmountRange > 0 begin
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
		--select @debtAmountFrom as '@debtAmountTo', @debtAmountTo as '@debtAmountTo'
				
		-- Цена продажи ------------------------------------
		declare @salePriceFrom money = 0
		declare @salePriceTo money = 1000000000
		if @SalePriceRange is not null and @SalePriceRange > 0 begin
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
		--select @salePriceFrom as '@salePriceFrom', @debtAmountTo as '@debtAmountTo'
		
		-- Регионы ------------------------------------
		declare @regions table (
			Id		uniqueidentifier	not null,
			Name	nvarchar(120)		not null
			primary key (Id)
		)
		if @RegionId is not null begin		
			insert @regions(Id, Name) 
			select r.AoGuid, r.FormalName + ' ' + r.ShortName from Regions r where r.AoGuid = @RegionId
		end else begin		
			insert @regions(Id, Name)
			select r.AoGuid, r.FormalName + ' ' + r.ShortName from Regions r
		end
		-- Регионы ------------------------------------
		--select * from @regions order by Name
		
		-- Решение суда ------------------------------------
		declare @courtDecisions table (
			Id		int				not null,
			Name	nvarchar(120)	not null
			primary key (Id)
		)
		if @CourtDecisionTypeId is not null and @CourtDecisionTypeId > 0 begin		
			insert @courtDecisions(Id, Name)
			select t.Id, t.Name from CourtDecisionTypes t where t.Id = @CourtDecisionTypeId
		end else begin		
			insert @courtDecisions(Id, Name)
			select t.Id, t.Name from CourtDecisionTypes t
		end
		-- Решение суда ------------------------------------
		--select * from @courtDecisions
		
		-- Должник ------------------------------------
		declare @debtors table (
			Id		int				not null,
			Name	nvarchar(120)	not null
			primary key (Id)
		)
		if @DebtorId is not null and @DebtorId > 0 begin		
			insert @debtors(Id, Name) 
			select t.Id, t.Name from DebtorTypes t where t.Id = @DebtorId
		end else begin		
			insert @debtors(Id, Name)
			select t.Id, t.Name from DebtorTypes t
		end
		-- Должник ------------------------------------
		--select * from @debtors
		
		-- Сущность долга ------------------------------------
		declare @debtEssences table (
			Id		int				not null,
			Name	nvarchar(120)	not null
			primary key (Id)
		)
		if @DebtEssenceTypeId is not null and @DebtEssenceTypeId > 0 begin		
			insert @debtEssences(Id, Name)
			select t.Id, t.Name from DebtEssenceTypes t where t.Id = @DebtEssenceTypeId
		end else begin		
			insert @debtEssences(Id, Name)
			select t.Id, t.Name from DebtEssenceTypes t
		end
		-- Сущность долга ------------------------------------
		--select * from @debtEssences
		
		-- Первоначальный кредитор ------------------------------------
		declare @originalCreditors table (
			Id		int				not null,
			Name	nvarchar(120)	not null
			primary key (Id)
		)
		if @OriginalCreditorTypeId is not null and @OriginalCreditorTypeId > 0 begin		
			insert @originalCreditors(Id, Name) 
			select t.Id, t.Name from OriginalCreditorTypes t where t.Id = @DebtEssenceTypeId
		end else begin		
			insert @originalCreditors(Id, Name)
			select t.Id, t.Name from OriginalCreditorTypes t
		end
		-- Первоначальный кредитор ------------------------------------
		--select * from @originalCreditors
		
		-- Продавец долга ------------------------------------
		declare @debtSellers table (
			Id		int				not null,
			Name	nvarchar(120)	not null
			primary key (Id)
		)
		if @DebtSellerTypeId is not null and @DebtSellerTypeId > 0 begin		
			insert @debtSellers(Id, Name) 
			select t.Id, t.Name from DebtSellerTypes t where t.Id = @DebtSellerTypeId
		end else begin		
			insert @debtSellers(Id, Name)
			select t.Id, t.Name from DebtSellerTypes t
		end
		-- Продавец долга ------------------------------------
		--select * from @debtSellers
		
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
					,r.Name as 'RegionName'
					,dt.Name as 'DebtorTypeName'
					,oc.Name as 'OriginalCreditorTypeName'
					,cd.Name as 'CourtDecisionTypeName'
					,s.Name as 'DebtSellerTypeName'
					,e.Name as 'DebtEssenceTypeName'					
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
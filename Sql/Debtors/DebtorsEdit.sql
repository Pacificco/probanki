-- -----------------------------------------------------------------------------------
--
-- Процедура редактирует записи в таблице должников
-- 
-- -----------------------------------------------------------------------------------
if exists(select 1 from sysobjects where name = N'DebtorsEdit' and xtype='P') drop proc DebtorsEdit
go
create proc DebtorsEdit (
	@Operation				int,					-- Тип операции: 1 - Создать, 2 - Изменить, 3 - Удалить, 4 - Архивировать, 5 - Закрыть должника
	@Id						int,					-- Идентификатор должника
	@DebtorTypeId			int,					-- Тип должника
	@Published				bit,					-- Опубликован
	@OriginalCreditorTypeId	int,					-- Первоначальный кредитор: Любой / Банк / МФО / юр.лицо / физ.лицо
	@RegionId				uniqueidentifier,		-- Регион должника: Все, перечень регионов в всплывающем окне
	@Locality				nvarchar,				-- Населенный пункт: вводится в ручную при заполнении
	@DebtEssenceTypeId		int,					-- Сущность долга: Любая / банковский кредит / займ МФО / частный долг / прочее
	@CourtDecisionTypeId	int,					-- Решение суда: Все / Есть / Нет
	@DebtCreatedDate		datetime,				-- Дата образования долга: За весь период (от ближайшей даты), месяц год в всплывающем окне
	@DebtAmount				money,					-- Сумма долга: вводится в ручную при заполнении (формат: ...ХХХ руб. ХХ коп.)
	@SalePrice				money,					-- Сумма долга: вводится в ручную при заполнении (формат: ...ХХХ руб. ХХ коп.)
	@DebtSellerTypeId		int,					-- Продавец долга: Любой / Банк / МФО / юр.лицо / физ.лицо
	@ContactPerson			nvarchar,				-- Контактное лицо: вводится в ручную при заполнении
	@ContactPhone			int,					-- Контактный телефон: вводится в ручную при заполнении
	@DopPhone				int,					-- Дополнительный контактный телефон: вводится в ручную при заполнении
	@Email					varchar,				-- Email: вводится в ручную при заполнении
	@Comment				nvarchar				-- Дополнительный комментарий	
) as	
begin	

	SET NOCOUNT ON;

	declare @trancount int
	select @trancount = @@TRANCOUNT

	begin try 
	
		if @trancount = 0
			begin transaction
		
		declare @curDate datetime
		select @curDate = GETDATE()
				
		if @Operation = 1 begin		-- Создаем новую запись
		
			insert Debtors(			
					DebtorTypeId
					,Published
					,OriginalCreditorTypeId
					,RegionId
					,Locality
					,DebtEssenceTypeId
					,CourtDecisionTypeId
					,DebtCreatedDate
					,DebtAmount
					,SalePrice
					,DebtSellerTypeId
					,ContactPerson
					,ContactPhone
					,DopPhone
					,Email
					,Comment
					,CreatedAt
					,UpdatedAt					
			)
			values(
				@DebtorTypeId
				,@Published
				,@OriginalCreditorTypeId
				,@RegionId
				,@Locality
				,@DebtEssenceTypeId
				,@CourtDecisionTypeId
				,@DebtCreatedDate
				,@DebtAmount
				,@SalePrice
				,@DebtSellerTypeId
				,@ContactPerson
				,@ContactPhone
				,@DopPhone
				,@Email
				,@Comment	
				,@curDate
				,@curDate
			)
				
		end else
		if @Operation = 2 begin		-- Изменяет существующую запись
			
			update Debtors set
					DebtorTypeId = @DebtorTypeId
					,Published = @Published
					,OriginalCreditorTypeId = @OriginalCreditorTypeId
					,RegionId = @RegionId
					,Locality = @Locality
					,DebtEssenceTypeId = @DebtEssenceTypeId
					,CourtDecisionTypeId = @CourtDecisionTypeId
					,DebtCreatedDate = @DebtCreatedDate
					,DebtAmount = @DebtAmount
					,SalePrice = @SalePrice
					,DebtSellerTypeId = @DebtSellerTypeId
					,ContactPerson = @ContactPerson
					,ContactPhone = @ContactPhone
					,DopPhone = @DopPhone
					,Email = @Email
					,Comment = @Comment					
					,UpdatedAt = @curDate					
			where Id = @Id
		
		end else
		if @Operation = 3 begin		-- Удаляем существующую запись
			
			-- Архивируем
			declare @result int = 0
			exec @result = DebtorsEdit 4,@Id,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null
			if @result = 1 begin
				if @trancount = 0	
					rollback transaction
				return 1
			end
			-- Удаляем
			delete Debtors where Id = @Id
			
		end else
		if @Operation = 4 begin		-- Архивирует существующую запись
			
			insert DebtorsArchive(			
					Id
					,DebtorTypeId
					,Published
					,OriginalCreditorTypeId
					,RegionId
					,Locality
					,DebtEssenceTypeId
					,CourtDecisionTypeId
					,DebtCreatedDate
					,DebtAmount
					,SalePrice
					,DebtSellerTypeId
					,ContactPerson
					,ContactPhone
					,DopPhone
					,Email
					,Comment
					,CreatedAt
					,UpdatedAt
					,ArchivedDate
			)
			select Id
					,DebtorTypeId
					,Published
					,OriginalCreditorTypeId
					,RegionId
					,Locality
					,DebtEssenceTypeId
					,CourtDecisionTypeId
					,DebtCreatedDate
					,DebtAmount
					,SalePrice
					,DebtSellerTypeId
					,ContactPerson
					,ContactPhone
					,DopPhone
					,Email
					,Comment
					,CreatedAt
					,UpdatedAt
					,@curDate
			From Debtors 
			where Id = @Id
			
			delete Debtors where Id = @Id
			
		end else
		if @Operation = 5 begin		-- Изменяет активность
			
			update Debtors set Published = @Published where Id = @Id
		
		end
	
		if @trancount = 0
			commit transaction
		
		return 0
	end try 
	begin catch 
		if @trancount = 0	
			rollback transaction
		return 1
	end catch 
end
go
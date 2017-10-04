-- -----------------------------------------------------------------------------------
--
-- ��������� ����������� ������ � ������� ���������
-- 
-- -----------------------------------------------------------------------------------
if exists(select 1 from sysobjects where name = N'DebtorsEdit' and xtype='P') drop proc DebtorsEdit
go
create proc DebtorsEdit (
	@Operation				int,					-- ��� ��������: 1 - �������, 2 - ��������, 3 - �������, 4 - ������������, 5 - ������� ��������
	@Id						int,					-- ������������� ��������
	@DebtorTypeId			int,					-- ��� ��������
	@Published				bit,					-- �����������
	@OriginalCreditorTypeId	int,					-- �������������� ��������: ����� / ���� / ��� / ��.���� / ���.����
	@RegionId				uniqueidentifier,		-- ������ ��������: ���, �������� �������� � ����������� ����
	@Locality				nvarchar,				-- ���������� �����: �������� � ������ ��� ����������
	@DebtEssenceTypeId		int,					-- �������� �����: ����� / ���������� ������ / ���� ��� / ������� ���� / ������
	@CourtDecisionTypeId	int,					-- ������� ����: ��� / ���� / ���
	@DebtCreatedDate		datetime,				-- ���� ����������� �����: �� ���� ������ (�� ��������� ����), ����� ��� � ����������� ����
	@DebtAmount				money,					-- ����� �����: �������� � ������ ��� ���������� (������: ...��� ���. �� ���.)
	@SalePrice				money,					-- ����� �����: �������� � ������ ��� ���������� (������: ...��� ���. �� ���.)
	@DebtSellerTypeId		int,					-- �������� �����: ����� / ���� / ��� / ��.���� / ���.����
	@ContactPerson			nvarchar,				-- ���������� ����: �������� � ������ ��� ����������
	@ContactPhone			int,					-- ���������� �������: �������� � ������ ��� ����������
	@DopPhone				int,					-- �������������� ���������� �������: �������� � ������ ��� ����������
	@Email					varchar,				-- Email: �������� � ������ ��� ����������
	@Comment				nvarchar				-- �������������� �����������	
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
				
		if @Operation = 1 begin		-- ������� ����� ������
		
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
		if @Operation = 2 begin		-- �������� ������������ ������
			
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
		if @Operation = 3 begin		-- ������� ������������ ������
			
			-- ����������
			declare @result int = 0
			exec @result = DebtorsEdit 4,@Id,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null
			if @result = 1 begin
				if @trancount = 0	
					rollback transaction
				return 1
			end
			-- �������
			delete Debtors where Id = @Id
			
		end else
		if @Operation = 4 begin		-- ���������� ������������ ������
			
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
		if @Operation = 5 begin		-- �������� ����������
			
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
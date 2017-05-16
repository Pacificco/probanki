-- ��������� ������ ������������ � ���� ������
if exists(select 1 from sysobjects where name = N'UpdateUserProfile' and xtype='P') drop proc UpdateUserProfile
go
create proc UpdateUserProfile (
	@UserId int,	
    @Name nvarchar(20),
    @Email nvarchar(20),
    @Sex int,
    @LastName nvarchar(20),
    @FatherName nvarchar(20),
    @IsSubscribed bit,
    @Nic nvarchar(20)
	) as
begin
	
	SET NOCOUNT ON;

	begin try 
		
		update Users 
		set	
			Name			= @Name,
			Email			= @Email,
			Sex				= @Sex,
			LastName		= @LastName,
			FatherName		= @FatherName,
			IsSubscribed	= @IsSubscribed,
			Nic				= @Nic
		where Id = @UserId
		
		return 0
	end try 
	begin catch 
		return 1
	end catch
    
end
go
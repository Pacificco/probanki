-- -----------------------------------------------------------------------------------
--
-- Таблица должников
-- 
-- -----------------------------------------------------------------------------------
--drop table Debtors
create table Debtors
(	
	Id						int	identity(1,1)		not null,						-- Идентификатор должника 
	DebtorTypeId			int						not null,						-- Должник
	Published				bit						not null	default(0),			-- Опубликован
	OriginalCreditorTypeId	int						not null,						-- Первоначальный кредитор: Любой / Банк / МФО / юр.лицо / физ.лицо
	RegionId				uniqueidentifier		not null,						-- Регион должника: Все, перечень регионов в всплывающем окне
	Locality				nvarchar(255)			not null,						-- Населенный пункт: вводится в ручную при заполнении
	DebtEssenceTypeId		int						not null,						-- Сущность долга: Любая / банковский кредит / займ МФО / частный долг / прочее
	CourtDecisionTypeId		int						not null,						-- Решение суда: Все / Есть / Нет
	DebtCreatedDate			datetime				not null,						-- Дата образования долга: За весь период (от ближайшей даты), месяц год в всплывающем окне
	DebtAmount				money					not null,						-- Сумма долга: вводится в ручную при заполнении (формат: ...ХХХ руб. ХХ коп.)
	SalePrice				money					not null,						-- Сумма долга: вводится в ручную при заполнении (формат: ...ХХХ руб. ХХ коп.)
	DebtSellerTypeId		int						not null,						-- Продавец долга: Любой / Банк / МФО / юр.лицо / физ.лицо
	ContactPerson			nvarchar(255)			not null,						-- Контактное лицо: вводится в ручную при заполнении										
	ContactPhone			bigint					not null,						-- Контактный телефон: вводится в ручную при заполнении
	DopPhone				bigint					null,							-- Дополнительный контактный телефон: вводится в ручную при заполнении
	Email					varchar(129)			not null,						-- Email: вводится в ручную при заполнении
	Comment					nvarchar(2000)			not null,						-- Дополнительный комментарий
	CreatedAt				datetime				not null	default(getdate()),	-- Дата создания записи в базе данных
	UpdatedAt				datetime				not null						-- Дата обновления записи в базе данных
	constraint PKDebtors primary key (Id)
)
go
create index IXDebtorsDebtCreatedDate on Debtors (DebtCreatedDate)
go


-- -----------------------------------------------------------------------------------
--
-- Таблица должников в архиве
-- 
-- -----------------------------------------------------------------------------------
--drop table DebtorsArchive
create table DebtorsArchive
(	
	Id						int						not null,						-- Идентификатор должника 
	DebtorTypeId			int						not null,						-- Должник
	Published				bit						not null	default(0),			-- Опубликован
	OriginalCreditorTypeId	int						not null,						-- Первоначальный кредитор: Любой / Банк / МФО / юр.лицо / физ.лицо
	RegionId				uniqueidentifier		not null,						-- Регион должника: Все, перечень регионов в всплывающем окне
	Locality				nvarchar(255)			not null,						-- Населенный пункт: вводится в ручную при заполнении
	DebtEssenceTypeId		int						not null,						-- Сущность долга: Любая / банковский кредит / займ МФО / частный долг / прочее
	CourtDecisionTypeId		int						not null,						-- Решение суда: Все / Есть / Нет
	DebtCreatedDate			datetime				not null,						-- Дата образования долга: За весь период (от ближайшей даты), месяц год в всплывающем окне
	DebtAmount				money					not null,						-- Сумма долга: вводится в ручную при заполнении (формат: ...ХХХ руб. ХХ коп.)
	SalePrice				money					not null,						-- Сумма долга: вводится в ручную при заполнении (формат: ...ХХХ руб. ХХ коп.)
	DebtSellerTypeId		int						not null,						-- Продавец долга: Любой / Банк / МФО / юр.лицо / физ.лицо
	ContactPerson			nvarchar(255)			not null,						-- Контактное лицо: вводится в ручную при заполнении										
	ContactPhone			bigint					not null,						-- Контактный телефон: вводится в ручную при заполнении
	DopPhone				bigint					null,							-- Дополнительный контактный телефон: вводится в ручную при заполнении
	Email					varchar(129)			not null,						-- Email: вводится в ручную при заполнении
	Comment					nvarchar(2000)			not null,						-- Дополнительный комментарий
	CreatedAt				datetime				not null	default(getdate()),	-- Дата создания записи в базе данных
	UpdatedAt				datetime				not null,						-- Дата обновления записи в базе данных	
	ArchivedDate			datetime				not null						-- Дата архивации
	constraint PKDebtorsArchive primary key (Id, ArchivedDate)
)
go
create index IXDebtorsArchiveDate on DebtorsArchive (ArchivedDate)
go


-- -----------------------------------------------------------------------------------
--
-- Таблица типов должников
-- 
-- -----------------------------------------------------------------------------------
-- drop table DebtorTypes
create table DebtorTypes
(
	Id		int	identity		not null,		-- Идентификатор типа
	Name	nvarchar(100)		not null		-- Наименование типа
	constraint PKDebtorTypes primary key (Id)
)
go
insert DebtorTypes(Name) values
('Юридическое лицо'),
('Физическое лицо')


-- -----------------------------------------------------------------------------------
--
-- Таблица типов первоначальных кредиторов
-- 
-- -----------------------------------------------------------------------------------
-- drop table OriginalCreditorTypes
create table OriginalCreditorTypes
(
	Id		int	identity		not null,		-- Идентификатор типа
	Name	nvarchar(100)		not null		-- Наименование типа
	constraint PKOriginalCreditorsTypes primary key (Id)
)
go
insert OriginalCreditorTypes(Name) values
('Банк'),
('МФО'),
('Юридическое лицо'),
('Физическое лицо')


-- -----------------------------------------------------------------------------------
--
-- Таблица типов сущностей долга
-- 
-- -----------------------------------------------------------------------------------
create table DebtEssenceTypes
(
	Id		int	identity		not null,		-- Идентификатор типа
	Name	nvarchar(100)		not null		-- Наименование типа
	constraint PKDebtEssenceTypes primary key (Id)
)
go
insert DebtEssenceTypes(Name) values
('Банковский кредит'),
('Займ МФО'),
('Частный долг'),
('Прочее')

-- -----------------------------------------------------------------------------------
--
-- Таблица типов решений суда
-- 
-- -----------------------------------------------------------------------------------
create table CourtDecisionTypes
(
	Id		int	identity		not null,		-- Идентификатор типа
	Name	nvarchar(100)		not null		-- Наименование типа
	constraint PKCourtDecisionTypes primary key (Id)
)
go
insert CourtDecisionTypes(Name) values
('Есть'),
('Нет')

-- -----------------------------------------------------------------------------------
--
-- Таблица типов продавцов долга	
-- 
-- -----------------------------------------------------------------------------------
create table DebtSellerTypes
(
	Id		int	identity		not null,		-- Идентификатор типа
	Name	nvarchar(100)		not null		-- Наименование типа
	constraint PKDebtSellerTypes primary key (Id)
)
go
insert DebtSellerTypes(Name) values
('Банк'),
('МФО'),
('Юридическое лицо'),
('Физическое лицо')

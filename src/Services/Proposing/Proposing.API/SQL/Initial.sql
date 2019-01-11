use [Sales.Proposing]
go

create table Region
(
	Id int not null,
	Name varchar(50) not null,
	constraint PK_Region primary key (Id),
)

create table Country
(
	Id int not null identity,
	Name varchar(50) not null,
	IsoCode char(2) not null,
	RegionId int not null,
	constraint PK_Country primary key (Id),
	constraint FK_Country_Region foreign key (RegionId) references Region(Id),
)

create table ComponentType
(
	Id smallint not null,
	ProductId int null,
	Name varchar(50) not null,
	FullName varchar(100) null,
	constraint PK_ComponentType primary key (Id)
)

create table Component
(
	Id smallint not null,
	ComponentTypeID smallint null,
	ProductId int null,
	Name varchar(50) not null,
	FullName varchar(100) null,
	IsActive bit not null,
	SortOrder tinyint null,
	constraint PK_Component primary key (Id),
	constraint FK_Component_ComponentType foreign key (ComponentTypeID) references ComponentType (Id)
)

create table Proposal
(
	Id int not null identity,
	Name varchar(100) not null,
	ClientName varchar(100) not null,
	Comments varchar(max) null,
	ProductModelId int not null,
	PriceModelId int not null,
	ProductIds bigint not null,
	constraint PK_Proposal primary key (Id),
)

create table ProposalCountry
(
	Id int not null identity,
	ProposalId int not null,
	CountryID int not null,
	ProductIds bigint not null,
	Headcount int null,
	constraint PK_ProposalCountry primary key (Id),
	constraint FK_ProposalCountry_Proposal foreign key (ProposalId) references Proposal (Id),
	constraint FK_ProposalCountry_Country foreign key (CountryId) references Country (Id),
)

create table PayrollScope
(
	Id int not null identity,
	ProposalId int not null,
	Reporting bit null,
	PayslipStorage bit null,
	constraint PK_PayrollScope primary key (Id),
	constraint FK_PayrollScope_Proposal foreign key (ProposalId) references Proposal (Id),
)

create table PayrollCountryScope
(
	Id int not null identity,
	ProposalId int not null,
	CountryID int not null,
	LevelId smallint not null,
	WeeklyPayees int not null,
	BiWeeklyPayees int not null,
	SemiMonthlyPayees int not null,
	MonthlyPayees int not null,
	Reporting bit null,
	PayslipStorage bit null,
	constraint PK_PayrollCountryScope primary key (Id),
	constraint FK_PayrollCountryScope_Proposal foreign key (ProposalId) references Proposal (Id),
	constraint FK_PayrollCountryScope_Component foreign key (LevelId) references Component (Id),
)

create table HrScope
(
	Id int not null identity,
	ProposalId int not null,
	LevelId smallint not null,
	constraint PK_HrScope primary key (Id),
	constraint FK_HrScope_Proposal foreign key (ProposalId) references Proposal (Id),
	constraint FK_HrScope_Component foreign key (LevelId) references Component (Id),
)

create table HrCountryScope
(
	Id int not null identity,
	ProposalId int not null,
	CountryID int not null,
	LevelId smallint not null,
	constraint PK_HrCountryScope primary key (Id),
	constraint FK_HrCountryScope_Proposal foreign key (ProposalId) references Proposal (Id),
)

/*
create table ProposalPrice
(
	Id int not null identity,
	ProposalId int not null,
	PriceItemId int not null,
	CountryId int null,
	TypeId tinyint not null,
	Book decimal(9,2) not null,
	Cost decimal(9,2) not null,
	Net decimal(9,2) not null,
	DiscountPct decimal(9,8) not null,
	Headcount int not null,
)
*/


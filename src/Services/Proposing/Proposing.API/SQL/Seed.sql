use [Sales.Proposing]
go

insert Region (Id, Name) values 
(1, 'APAC'),
(2, 'EMEA'),
(8, 'N. America'),
(16, 'Lat. America')

set identity_insert Country on

insert Country (Id, Name, IsoCode, RegionId)
select	CountryId, Name, IsoCode, Version2RegionId
from	Pricing.dbo.Country

set identity_insert Country off

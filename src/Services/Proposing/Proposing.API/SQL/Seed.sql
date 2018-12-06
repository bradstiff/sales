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

insert Component values (0, null, null, 'None', 'None', 1,  0)

--Payroll
--select @componentTypeID = 1
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 8, 'Connectivity Option')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (1, @componentTypeID, 8, 'GDMS', 'ADP Global Data Management System', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (2, @componentTypeID, 8, 'SDH', 'ADP StreamOnline Data Hub', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (3, @componentTypeID, 8, 'GVS', 'ADP GVS Integration', 0, 30)

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (7, null, 8, 'SL', 'ADP Streamline Payroll Service', 1, 10)

declare @componentTypeId smallint
select @componentTypeID = 2
insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 1, 'Payroll Level')

insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (4, @componentTypeID, 1, 'PS', 'Processing Service', 1, 10)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (5, @componentTypeID, 1, 'MS', 'Managed Service', 1, 20)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (6, @componentTypeID, 1, 'COS', 'Comprehensive Service', 1, 30)

----GV HCM Payroll Features
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (8, null, 9, 'ePayslips', 'Electronic Payslips', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (9, null, 9, 'Mobile', 'Mobile Solutions', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (10, null, 9, 'Dashboard', 'ADP Insight Dashboard', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (11, null, 9, 'Analytics', 'ADP GlobalView Analytics', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (21, null, 9, 'SSO', 'Single Sign On', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (22, null, 9, 'Payslip Storage', 'Payslip Storage', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (23, null, 9, 'Document Management', 'Document Management', 1, 0)

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (24, null, 1, 'Interfaces', 'Extra Interfaces', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (25, null, 1, 'Ancillaries', 'Bundled Ancillary Services', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (29, null, 1, 'Print', 'Printed Payslips', 1, 0)

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (26, null, 8, 'UHRA', 'UHRA', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (27, null, 8, 'UHRA Lite', 'UHRA Lite', 1, 0)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (28, null, 8, 'BI', 'Business Intelligence', 1, 0)

select @componentTypeID = 3
insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 9, 'Pay Frequencies')

insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (51, @componentTypeID, 9, 'Monthly', 'Monthly', 1, 10)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (52, @componentTypeID, 9, 'Lunar', 'Lunar', 1, 20)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (53, @componentTypeID, 9, 'Semi-monthly', 'Semi-monthly', 1, 30)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (54, @componentTypeID, 9, 'Bi-weekly', 'Bi-weekly', 1, 40)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (55, @componentTypeID, 9, 'Weekly', 'Weekly', 1, 50)

--eTime
select @componentTypeID = 4
insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 64, 'Time Bundle')

insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (31, @componentTypeID, 64, 'Absence Mgmt', 'Absence Management', 1, 10)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (32, @componentTypeID, 64, 'Time & Absence Mgmt', 'Time & Absence Management', 1, 20)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (33, @componentTypeID, 64, 'Attendance Mgmt', 'Attendance Management', 1, 30)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (34, @componentTypeID, 64, 'Comprehensive Leave Mgmt', 'Comprehensive Leave Management', 1, 40)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (35, @componentTypeID, 64, 'Comprehensive Time & Labor Mgmt', 'Comprehensive Time & Labor Management', 1, 50)

--select @componentTypeID = 5
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 64, 'Clock Type')

--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (61, @componentTypeID, 64, 'Barcode', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (62, @componentTypeID, 64, 'Barcode w/biometric', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (63, @componentTypeID, 64, 'Proximity', 1, 30)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (64, @componentTypeID, 64, 'Proximity w/biometric', 1, 40)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (65, @componentTypeID, 64, 'Mag Strip', 1, 50)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (66, @componentTypeID, 64, 'Mag Strip w/biometric', 1, 60)

--select @componentTypeID = 6
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 64, 'License')

--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (71, @componentTypeID, 64, 'Core', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (72, @componentTypeID, 64, 'Manager', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (73, @componentTypeID, 64, 'Accruals', 1, 30)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (74, @componentTypeID, 64, 'Time Entry Employee', 1, 40)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (75, @componentTypeID, 64, 'Leave', 1, 50)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (76, @componentTypeID, 64, 'Attendance', 1, 60)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (77, @componentTypeID, 64, 'Advanced Scheduler', 1, 70)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (78, @componentTypeID, 64, 'Activities', 1, 80)
--insert	Component(Id, ComponentTypeID, ProductId, Name, IsActive, SortOrder) values (79, @componentTypeID, 64, 'Integration Mgr', 1, 90)

--select @componentTypeID = 7
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 1, 'Data Exception & Validation Option')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (38, @componentTypeID, 1, 'Tool', 'Data Exception & Validation Tool', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (39, @componentTypeID, 1, 'Service', 'Data Exception & Validation Service', 1, 20)

--select @componentTypeID = 8
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 64, 'EeT Clock Model')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (69, @componentTypeID, 64, 'Subscription', 'Clock Subscription', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (70, @componentTypeID, 64, 'Purchase', 'Purchased Clocks', 1, 20)

--select @componentTypeID = 9
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 9, 'eSocial Option')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (41, @componentTypeID, 1, 'Basic', 'ADP Basic eSocial Service', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (42, @componentTypeID, 1, 'Full', 'ADP Full eSocial Service', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (43, @componentTypeID, 8, 'Streamline', 'ADP eSocial Service', 1, 30)

select @componentTypeID = 10
insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 4, 'HR Level')

insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (13, @componentTypeID, 4, 'Personnel Admin', 'Personnel Administration', 1, 20)
insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (14, @componentTypeID, 4, 'Organizational Mgmt', 'Organizational Management', 1, 30)

--select @componentTypeID = 11
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 2, 'GV Time Option')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (17, @componentTypeID, 2, 'Absence Mgmt', 'ADP GlobalView Absence Management', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (18, @componentTypeID, 2, 'Basic Time Mgmt', 'ADP GlobalView Basic Time Management', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (19, @componentTypeID, 2, 'Time Level 3', 'ADP GlobalView Time Level 3', 0, 30)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (20, @componentTypeID, 2, 'Time Mgmt', 'ADP GlobalView Time Management', 1, 40)

--select @componentTypeID = 12
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, null, 'Language')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (101, @componentTypeID, null, 'English', 'English', 1, 10)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (102, @componentTypeID, null, 'French', 'French', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (103, @componentTypeID, null, 'Spanish', 'Spanish', 1, 30)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (104, @componentTypeID, null, 'German', 'German', 1, 40)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (105, @componentTypeID, null, 'Dutch', 'Dutch', 1, 50)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (106, @componentTypeID, null, 'Italian', 'Italian', 1, 60)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (107, @componentTypeID, null, 'Portuguese (BR)', 'Portuguese (BR)', 1, 70)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (108, @componentTypeID, null, 'Chinese', 'Chinese', 1, 80)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (109, @componentTypeID, null, 'Polish', 'Polish', 1, 90)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (110, @componentTypeID, null, 'Korean', 'Korean', 1, 100)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (111, @componentTypeID, null, 'Japanese', 'Japanese', 1, 110)

--select @componentTypeID = 13
--insert	ComponentType (Id, ProductId, Name) values (@componentTypeID, 4, 'GV HR Setup Method')

--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (81, @componentTypeID, 4, 'Country', 'Country', 1, 20)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (82, @componentTypeID, 4, 'Cluster', 'Cluster', 1, 30)
--insert	Component(Id, ComponentTypeID, ProductId, Name, FullName, IsActive, SortOrder) values (83, @componentTypeID, 4, 'Big Bang', 'Big Bang', 1, 40)


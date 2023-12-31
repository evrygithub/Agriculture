USE [master]
GO
/****** Object:  Database [Agriculture]    Script Date: 8/19/2023 2:32:13 PM ******/
CREATE DATABASE [Agriculture]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Agriculture', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Agriculture.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Agriculture_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\Agriculture_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [Agriculture] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Agriculture].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Agriculture] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Agriculture] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Agriculture] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Agriculture] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Agriculture] SET ARITHABORT OFF 
GO
ALTER DATABASE [Agriculture] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Agriculture] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Agriculture] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Agriculture] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Agriculture] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Agriculture] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Agriculture] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Agriculture] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Agriculture] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Agriculture] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Agriculture] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Agriculture] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Agriculture] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Agriculture] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Agriculture] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Agriculture] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Agriculture] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Agriculture] SET RECOVERY FULL 
GO
ALTER DATABASE [Agriculture] SET  MULTI_USER 
GO
ALTER DATABASE [Agriculture] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Agriculture] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Agriculture] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Agriculture] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Agriculture] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Agriculture', N'ON'
GO
ALTER DATABASE [Agriculture] SET QUERY_STORE = OFF
GO
USE [Agriculture]
GO
/****** Object:  Table [dbo].[Crop]    Script Date: 8/19/2023 2:32:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Crop](
	[ID] [varchar](100) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[StatusID] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_Crop] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Crop] ADD  CONSTRAINT [DF_Crop_ID]  DEFAULT (newid()) FOR [ID]
GO
/****** Object:  StoredProcedure [dbo].[Sp_DeleteCrop]    Script Date: 8/19/2023 2:32:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sashidar
-- Create date: 19-Aug-2023
-- Description:	Delete Crop
-- =============================================
CREATE PROCEDURE [dbo].[Sp_DeleteCrop]
	@ID VARCHAR(100)
AS
BEGIN
    -- Delete Crop where record is Active by using update query to perfrom soft delete.
	-- As a IT rule, Transaction tables do not allow hard delete for any reocord.
	-- StatusID 3 means soft delete.
	UPDATE Crop SET StatusID=3,ModifiedDate=GETDATE() WHERE ID =@ID AND StatusID=1
	SELECT @ID AS ID
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetAllCrops]    Script Date: 8/19/2023 2:32:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sashidar
-- Create date: 19-Aug-2023
-- Description:	Get All Crops
-- =============================================
CREATE PROCEDURE [dbo].[Sp_GetAllCrops]
	
AS
BEGIN
    -- ACTIVE Records of all crops.
	SELECT ID, [Name], StatusID, CreatedDate, ModifiedDate FROM Crop WHERE StatusID=1
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_GetCropByID]    Script Date: 8/19/2023 2:32:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sashidar
-- Create date: 19-Aug-2023
-- Description:	Get Crop By ID
-- =============================================
CREATE PROCEDURE [dbo].[Sp_GetCropByID]
	@ID VARCHAR(100)
AS
BEGIN
    -- ACTIVE Record of single crop based on Primary Key column.
	SELECT ID, [Name], StatusID, CreatedDate, ModifiedDate FROM Crop WHERE ID=@ID AND StatusID=1
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_InsertCrop]    Script Date: 8/19/2023 2:32:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sashidar
-- Create date: 19-Aug-2023
-- Description:	Insert Crop
-- ============================================= 

-- https://techfunda.com/howto/192/transaction-in-stored-procedure

CREATE PROCEDURE [dbo].[Sp_InsertCrop]
       -- Add the parameters for the stored procedure here
       @Name varchar(100),
       @StatusID int
       
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;
	   DECLARE @ID VARCHAR(100)
	   SET @ID=NEWID(); -- Setting value for Primary Key column ID by using SQL Built in method.
       BEGIN TRAN Trans
              BEGIN TRY
			  IF NOT EXISTS(SELECT TOP 1 ID FROM Crop WHERE Name=@Name AND StatusID=1)
			  BEGIN
                     -- Insert into Crop Table
                     INSERT INTO Crop
                            (ID, [Name], StatusID, CreatedDate, ModifiedDate)
                     VALUES
                            (@ID, @Name, @StatusID, GETDATE(),GETDATE())

                     -- IF no error, commit the transcation
					 If(@@TRANCOUNT >0)
					 BEGIN
					 COMMIT TRANSACTION
					 END
				    SELECT @ID AS ID -- Return Primary Key Column value.
			 END
			 ELSE
			BEGIN
					SELECT '0' AS ID  -- Return zero means, record already exists
			 END
       END TRY
       BEGIN CATCH
              -- if error, roll back any chanegs done by any of the sql statements
			  If(@@TRANCOUNT >0)
					 BEGIN
					 ROLLBACK TRANSACTION
					 END
            SELECT '-1' AS ID   -- Error in query execution
       END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_UpdateCrop]    Script Date: 8/19/2023 2:32:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sashidar
-- Create date: 19-Aug-2023
-- Description:	Update Crop
-- ============================================= 

-- https://techfunda.com/howto/192/transaction-in-stored-procedure

CREATE PROCEDURE [dbo].[Sp_UpdateCrop]
       -- Add the parameters for the stored procedure here
	   @ID VARCHAR(100),
       @Name VARCHAR(100),
       @StatusID int
       
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;
	   BEGIN TRAN Trans
              BEGIN TRY
			  IF EXISTS(SELECT TOP 1 ID FROM Crop WHERE ID=@ID  AND StatusID=1)
			    BEGIN
                     -- Update into Crop Table
					 IF NOT EXISTS(SELECT TOP 1 ID FROM Crop WHERE [Name]=@Name AND StatusID=1)
					 BEGIN
					 UPDATE CROP SET [Name]=@Name, StatusID=@StatusID, ModifiedDate=GETDATE() WHERE ID=@ID AND StatusID=1
					 END
					 ELSE
					 BEGIN
						SELECT '0' AS ID  -- Return zero means, record already exists
					 END
                     -- If no error, commit the transcation
					 If(@@TRANCOUNT >0)
					 BEGIN
					 COMMIT TRANSACTION
					 END
					SELECT @ID AS ID -- Return Primary Key Column value.
			    END
			 --ELSE
				-- BEGIN
				--	SELECT '0' AS ID  -- Return zero means, record already exists
				-- END
       END TRY
       BEGIN CATCH
              -- if error, roll back any chanegs done by any of the sql statements
			  If(@@TRANCOUNT >0)
					 BEGIN
					 ROLLBACK TRANSACTION
					 END
            SELECT '-1' AS ID   -- Error in query execution
       END CATCH
END
GO
USE [master]
GO
ALTER DATABASE [Agriculture] SET  READ_WRITE 
GO

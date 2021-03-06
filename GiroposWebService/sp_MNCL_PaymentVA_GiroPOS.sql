USE [MNC_GUI_SSS]
GO
/****** Object:  StoredProcedure [dbo].[sp_MNCL_PaymentVA_GiroPOS]    Script Date: 14/02/2022 14:29:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_MNCL_PaymentVA_GiroPOS]
	@VA_NUMB VARCHAR(50),
	@AMOUNT DECIMAL,
	@ADMINFEE DECIMAL,
	@REF_NUMB VARCHAR(20),
	@WAKTU_PROSES VARCHAR(50)
AS
BEGIN

	DECLARE @LSAGREE VARCHAR(20)
	DECLARE @NAMA VARCHAR(100)

	DECLARE @strError VARCHAR(50) = ''
	DECLARE @strErrorCode VARCHAR(50) = 'ERROR '
	DECLARE @strErrorMsg VARCHAR(2000) = ''

	--GET VA Number
	SELECT 
		@NAMA = SC.NAME,
		@LSAGREE = LA.LSAGREE
	FROM LS_AGREEMENT LA 
	JOIN SYS_CLIENT SC ON SC.CLIENT =  LA.LESSEE
	INNER JOIN PAYMENT_POINT_VA VA ON LA.LSAGREE = VA.LSAGREE AND VA.PAYMENT_POINT_CODE = 'GIROPOS'
	WHERE 1=1
		AND VA.VIRTUAL_ACCNO = @VA_NUMB
	
	IF (@LSAGREE IS NULL)
		BEGIN
			SET @strErrorCode += '9614'
			SET @strErrorMsg = @strErrorCode + ': ' + @VA_NUMB + ' VA Number does not exist'
			RAISERROR(@strErrorMsg, 16, 1)
		END
	ELSE
		BEGIN
			--CREATE PAYMENT
			INSERT INTO TEMP_PAY_CHANNEL_RCV (REF_NO,VIRTUAL_ACC_NO,PAY_AMT,TEMP_PAY_CHANNEL_RCV_STAT,VALUE_DT,POST_DT,CRE_BY,CRE_DT,MOD_BY,MOD_DT,CRE_IP_ADDR,MOD_IP_ADDR, PAY_SOURCE, TRANSACTION_TYPE)
			VALUES (@REF_NUMB,@VA_NUMB,@AMOUNT,'NEW',GETDATE(),GETDATE(),'WS VA',GETDATE(),'WS VA',GETDATE(),'127.0.0.1','127.0.0.1','POS','Payment')
			
			INSERT INTO [dbo].[TRX_GIROPOS_LOG]
			   ([trx_type]
			   ,[no_va]
			   ,[nama]
			   ,[nominal]
			   ,[adminfee]
			   ,[ref_no]
			   ,[waktu_proses]
			   ,[CRE_BY]
			   ,[CRE_DT])
			VALUES
				('Payment',
				@VA_NUMB,
				@NAMA,
				@AMOUNT,
				@ADMINFEE,
				@REF_NUMB,
				@WAKTU_PROSES,
				'system',
				GETDATE())
		END
END



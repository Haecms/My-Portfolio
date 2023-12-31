USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP07_BM_ImageMaster_U1]    Script Date: 2023-09-04 오후 6:18:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-08-15
-- Description:	이미지 마스터 저장 프로시저
-- =============================================
ALTER PROCEDURE [dbo].[SP07_BM_ImageMaster_U1] 
	@ITEMCODE  VARCHAR(20),
	@PLANTCODE VARCHAR(20),
	@EDITOR    VARCHAR(20),
	@IMAGE     IMAGE,

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT


AS
BEGIN

	DECLARE @LD_NOWDATE DATETIME
	    SET @LD_NOWDATE = GETDATE()

	UPDATE TB_ItemMaster
	   SET IMGSRC   = @IMAGE
	      ,EDITOR   = @EDITOR
		  ,EDITDATE = @LD_NOWDATE
	 WHERE ITEMCODE  = @ITEMCODE
	   AND PLANTCODE = @PLANTCODE
END

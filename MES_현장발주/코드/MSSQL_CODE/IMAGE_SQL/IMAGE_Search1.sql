USE [KDTB03_1JO]
GO
/****** Object:  StoredProcedure [dbo].[SP07_BM_ImageMaster_S1]    Script Date: 2023-09-04 오후 6:17:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		조해찬
-- Create date: 2023-08-14
-- Description:	이미지 마스터 조회 프로시저 
-- =============================================
ALTER PROCEDURE [dbo].[SP07_BM_ImageMaster_S1]
	@PLANTCODE VARCHAR(20),  -- 공장     
	@ITEMNAME  VARCHAR(20),  -- 품명
	@ITEMTYPE  VARCHAR(20),  -- 품목타입

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT
AS
BEGIN
	IF(@ITEMTYPE = '') -- 전체선택
	BEGIN
		SELECT PLANTCODE				    AS PLANTCODE
			  ,'['+ITEMTYPE+']'+B.CODENAME  AS ITEMTYPE
			  ,ITEMCODE					    AS ITEMCODE	  
			  ,ITEMNAME					    AS ITEMNAME
			  ,BASEUNIT					    AS UNITCODE
			  ,ITEMSPEC					    AS ITEMSPEC
			  ,DBO.FN_GET_USERNAME(EDITOR)  AS EDITOR  
			  ,EDITDATE					    AS EDITDATE
		  FROM TB_ItemMaster A WITH(NOLOCK) LEFT JOIN (SELECT MINORCODE
												             ,CODENAME
												 	     FROM TB_Standard
												 	    WHERE MAJORCODE = 'ITEMTYPE') B
												   ON A.ITEMTYPE = B.MINORCODE
		 WHERE PLANTCODE LIKE '%' + @PLANTCODE + '%'
		   AND ITEMNAME  LIKE '%' + @ITEMNAME + '%'
		   AND ITEMTYPE  LIKE '%' + @ITEMTYPE + '%'
	  ORDER BY A.PLANTCODE, A.ITEMTYPE, A.ITEMCODE
	END
	ELSE
	BEGIN
		SELECT PLANTCODE					AS PLANTCODE
			  ,'['+ITEMTYPE+']'+B.CODENAME	AS ITEMTYPE
			  ,ITEMCODE						AS ITEMCODE	  
			  ,ITEMNAME						AS ITEMNAME
			  ,BASEUNIT						AS UNITCODE
			  ,ITEMSPEC						AS ITEMSPEC
			  ,DBO.FN_GET_USERNAME(EDITOR)  AS EDITOR  
			  ,EDITDATE						AS EDITDATE
		  FROM TB_ItemMaster A WITH(NOLOCK) LEFT JOIN (SELECT MINORCODE
												             ,CODENAME
												 	     FROM TB_Standard
												 	    WHERE MAJORCODE = 'ITEMTYPE'
												 	      AND MINORCODE = @ITEMTYPE) B
												   ON A.ITEMTYPE = B.MINORCODE
		                                           
		 WHERE PLANTCODE LIKE '%' + @PLANTCODE + '%'
		   AND ITEMNAME  LIKE '%' + @ITEMNAME + '%'
		   AND ITEMTYPE = @ITEMTYPE
      ORDER BY A.PLANTCODE, A.ITEMTYPE, A.ITEMCODE
	END
END

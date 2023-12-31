USE [KDTB03_MES_1]
GO
/****** Object:  StoredProcedure [dbo].[SP07_QA_POP_S1]    Script Date: 2023-08-31 오전 2:23:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		김건희
-- Create date: <Create Date,,>
-- Description:	그리드 조회
-- =============================================
ALTER PROCEDURE [dbo].[SP07_QA_POP_S1]

	@CHKCODE   VARCHAR(30),

	@LANG      VARCHAR(10),  
	@RS_CODE   VARCHAR(1)   OUTPUT,
	@RS_MSG    VARCHAR(100) OUTPUT

AS
BEGIN
 SELECT			 MATLOTNO                   AS     MATLOTNO       
	            ,PLANTCODE                  AS     PLANTCODE     
	       	    ,WORKCENTERCODE             AS	   WORKCENTERCODE
	       	    ,A.CHKCODE                    AS     CHKCODE         
--	       	    ,SEQ                        AS     SEQ                 
	       	    ,ITEMCODE                   AS     ITEMCODE       
	       	    ,ITEMNAME                   AS     ITEMNAME       
--	       	    ,MAKER                      AS     MAKER             
--	       	    ,MAKEDATE                   AS     MAKEDATE
			    ,SUM(A.BADWAITINGNO)             AS     BADWAITINGNO
				,B.PASSNO				AS     PASSNO
	        FROM  P_Combine_B A  WITH(NOLOCK) LEFT JOIN P_Lot B WITH(NOLOCK)
													 ON A.CHKCODE = B.CHKCODE
												    AND C_STATE = '합'
	       WHERE  A.CHKCODE = @CHKCODE
		GROUP BY MATLOTNO,PLANTCODE,WORKCENTERCODE,A.CHKCODE,ITEMCODE ,ITEMNAME,B.PASSNO

END
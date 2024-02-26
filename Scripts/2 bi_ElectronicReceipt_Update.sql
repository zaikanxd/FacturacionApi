USE Billing
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE bi_ElectronicReceipt_Update
	@id INT,
	@acceptedBySunat BIT,
	@sunatDescription VARCHAR(200) = NULL,
	@errorMessage VARCHAR(200) = NULL,
    @cdrTicketNumber VARCHAR(50) = NULL
AS

UPDATE ElectronicReceipt SET
	acceptedBySunat = @acceptedBySunat,
	sunatDescription = @sunatDescription,
	errorMessage = @errorMessage,
	cdrTicketNumber = @cdrTicketNumber,
	updateDate = GETDATE()
WHERE id = @id
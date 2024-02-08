using BusinessEntity;
using BusinessEntity.Error;
using InterfaceData;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Data.Common;

namespace DataAccess
{
    public class AuthenticationDA : IAuthenticationDA
    {
        private Database _db;
        public Database db { get { return (_db == null ? _db = new SqlDatabase(Util.AppSettings.cnxBillingBD) : _db); } }

        public AuthResponse login(AuthRequest pAuthRequest)
        {
            AuthResponse oAuthResponse = new AuthResponse();

            string errorMessage = null;
            string encryptedPassword = Util.Encrypt.GetSHA256(pAuthRequest.password);

            using (DbCommand cmd = db.GetStoredProcCommand(Util.GetNameStoreProcedure.bi_User_Login))
            {

                cmd.CommandTimeout = 0;
                db.AddInParameter(cmd, "username", System.Data.DbType.String, pAuthRequest.username);
                db.AddInParameter(cmd, "password", System.Data.DbType.String, encryptedPassword);
                db.AddOutParameter(cmd, "errorMessage", System.Data.DbType.String, 4000);
                db.ExecuteNonQuery(cmd);

                if (db.GetParameterValue(cmd, "errorMessage") != DBNull.Value)
                {
                    errorMessage = Convert.ToString(db.GetParameterValue(cmd, "errorMessage"));
                }
            }

            if (errorMessage != null)
            {
                throw new CustomException(errorMessage);
            }

            Token token = TokenGenerator.GenerateTokenJwt(pAuthRequest.username);
            oAuthResponse.token = token.jwtToken;
            oAuthResponse.expires = token.expires;

            return oAuthResponse;
        }
    }
}

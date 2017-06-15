using System;
using System.Security.Principal;

namespace SampleWebProject.Infrastructure.Security {

    public class HubIdentity : IIdentity {
        private readonly int _userId;
        private readonly string _name;
        private readonly int _companyId;
        private readonly DateTime _expireDate;

        public HubIdentity(int userId, string name, int companyId, DateTime expireDate) {
            _userId = userId;
            _name = name;
            _companyId = companyId;
            _expireDate = expireDate;
        }

        public int UserId {
            get { return _userId; }
        }

        public string Name {
            get { return _name; }
        }

        public int CompanyId {
            get { return _companyId; }
        }

        public DateTime ExpireDate {
            get { return _expireDate; }
        }

        public string AuthenticationType {
            get { return "HubSecurity"; }
        }

        public bool IsAuthenticated {
            get { return true; }
        }

        #region Request Info

        public string ClientTraceId { get; set; }

        public string EchoToken { get; set; }

        private Guid? _traceId;
        private readonly object _traceIdLocker = new object();
        public Guid? TraceId {
            get {
                if (_traceId == null) {
                    lock (_traceIdLocker) {
                        if (_traceId == null)
                            _traceId = Guid.NewGuid();
                    }
                }

                return _traceId;
            }
        }

        public string BookingCode { get; set; }

        public string Action { get; set; }

        #endregion

        public string CurrentTransactionIdetifier { get; set; }
    }
}

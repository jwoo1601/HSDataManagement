using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HyosungManagement.ViewModels
{
    public class ModalViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string FormName { get; set; }
        public string TriggerAction { get; set; }
        public string Method { get; set; }
        public string SubmitUrl { get; set; }

        public ModalViewModel Attribute(string name)
        {
            attributes.Add(name);
            return this;
        }

        public bool HasAttribute(string name)
        {
            return attributes.Contains(name);
        }

        private ISet<string> attributes = new HashSet<string>();
    }
}

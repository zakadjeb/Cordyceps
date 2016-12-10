using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Cordyceps
{
    public class CordycepsInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Cordyceps";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return new System.Drawing.Bitmap("c:/users/zadj/desktop/new_cordyceps.png");
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "Reading the module.";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("89ef2864-b9d4-4b30-ba68-d822faf44152");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "Aalborg University";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "Zakaria Djebbara - zadj@create.aau.dk";
            }
        }
    }
}

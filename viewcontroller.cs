using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace dropdown
{
    public partial class ViewController : UIViewController , IUITableViewDelegate ,IUITableViewDataSource ,IUITextFieldDelegate 
    {
        private readonly string[] item = new string[]
        {
            "male","female"
        };

        private UITableView _dropDownTableView;
        private UIView _dropDownView;

        [Export("tableView:numberOfRowsInSection:")]
        public nint RowsInSection(UITableView tableView, nint section)
        {
            return item.Length;
        }

        [Export("tableView:cellForRowAtIndexPath:")]
        public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell("DropDownCell");
            cell.TextLabel.Text = item[indexPath.Row];
            return cell;
        }

        [Export("tableView:viewForFooterInSection:")]
        public UIView GetViewForFooter(UITableView tableView, nint section)
        {
            UIView footerView = new UIView(new CGRect(0, 0, 0, 0));
            return footerView;
        }
        [Export("tableView:heightForFooterInSection:")]
        public nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return 1;
        }
        private void CreateDropDownView(CGRect frameForDropDown)
        {
            _dropDownView = new UIView(frameForDropDown);
            _dropDownTableView = new UITableView(new CGRect(0, 0, frameForDropDown.Width, frameForDropDown.Height));
            _dropDownTableView.RegisterClassForCellReuse(typeof(UITableViewCell), "DropDownCell");
            _dropDownTableView.DataSource = this;
            _dropDownTableView.Delegate = this;
            dropTextField.Delegate = this;
            _dropDownView.AddSubview(_dropDownTableView);
            AddShadowToDropDown();
        }
        private void AddShadowToDropDown()
        {
            var shadowPath = UIBezierPath.FromRect(_dropDownView.Bounds);
            _dropDownView.Layer.MasksToBounds = false;
            _dropDownView.Layer.ShadowColor = UIColor.Black.CGColor;
            _dropDownView.Layer.ShadowOffset = new CGSize(width: 0, height: 0.5);
            _dropDownView.Layer.ShadowOpacity = 0.2f;
            _dropDownView.Layer.ShadowPath = shadowPath.CGPath;
            _dropDownTableView.ClipsToBounds = true;
        }
        [Export("textFieldShouldBeginEditing:")]
        public bool ShouldBeginEditing(UITextField textField)
        {
            View.AddSubview(_dropDownView);
            UIApplication.SharedApplication.KeyWindow.BringSubviewToFront(_dropDownTableView);
            return false;
        }
        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            dropTextField.Text = item[indexPath.Row];
            _dropDownView.RemoveFromSuperview();
        }




        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CreateDropDownView(new CGRect(dropTextField.Frame.X,dropTextField.Frame.Y,
                dropTextField.Frame.Width,43 * item.Length));
       
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}

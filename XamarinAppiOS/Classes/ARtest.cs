using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARKit;
using Foundation;
using SceneKit;
using UIKit;

namespace XamarinAppiOS.Classes
{

    public partial class ViewController : UIViewController
    {
        private readonly ARSCNView sceneView;

        /// <summary>
        /// this creates a blank scene to ensure the camera feature of the mobile device is activated
        /// </summary>
        /// <param name="handle">variable name given to connect with the iOS API</param>
        public ViewController(IntPtr handle) : base(handle)
        {

            this.sceneView = new ARSCNView
            {
                AutoenablesDefaultLighting = true,
                DebugOptions =
                ARSCNDebugOptions.ShowFeaturePoints | ARSCNDebugOptions.ShowWorldOrigin
            };

            this.View.AddSubview(this.sceneView);
        }

        /// <summary>
        /// abstract class that hands down the ViewDidLoad() for child methods to inherit
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.sceneView.Frame = this.View.Frame;
        }

        /// <summary>
        /// Boolean method for animation and adding text to the scene
        /// </summary>
        /// <param name="animated">animated is either true or false</param>
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);

            //Add text to scene
            var textNode = new TextNode("Hello Universe", UIColor.Orange)
            {
                Position = new SCNVector3(0, -0.6f, 0) // Specifies position that the 3D text appears. Any lower than this and it doesn't show.
            };

            this.sceneView.Scene.RootNode.AddChildNode(textNode);
        }

        /// <summary>
        /// This boolean method is the opposite of the previous method and allows the view to disappear if bool is true.
        /// </summary>
        /// <param name="animated">animated is either true or false </param>
        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.sceneView.Session.Pause();
        }

        /// <summary>
        /// This method handles error messaging
        /// </summary>
        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }

   
    public class TextNode : SCNNode
    {   
        /// <summary>
        /// This method is called from the ViewDidAppear method above 
        /// </summary>
        /// <param name="text">string of text</param>
        /// <param name="color">color of text</param>
        public TextNode(string text, UIColor color)
        {
            var rootNode = new SCNNode
            {
                Geometry = CreateGeometry(text, color),
                Position = new SCNVector3(0, 0, 0)
            };

            AddChildNode(rootNode);
        }

        /// <summary>
        /// Geometry to create the 3D effect
        /// </summary>
        /// <param name="text">string text</param>
        /// <param name="color">color of text</param>
        /// <returns>3D text in specified color</returns>
        private static SCNGeometry CreateGeometry(string text, UIColor color)
        {
            var geometry = SCNText.Create(text, 0.01f);
            geometry.Font = UIFont.FromName("Courier", 0.5f);
            geometry.Flatness = 0;
            geometry.FirstMaterial.DoubleSided = true;
            geometry.FirstMaterial.Diffuse.Contents = color;
            geometry.FirstMaterial.Specular.Contents = UIColor.Blue;
            return geometry;
        }
    }
}
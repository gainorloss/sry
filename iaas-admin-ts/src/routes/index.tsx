import { RouteObject, useNavigate } from "react-router-dom";
import App from "../App"
import ErrorPage from "../ErrorPage"
import Home from "../pages/Home"
import Product from "../pages/pc/spu/Product";
import Login from "../pages/Login";

const routes:RouteObject[] = [{
    element: <App />,
    // Component: App,
    // errorElement: <ErrorPage />,
    // ErrorBoundary: ErrorPage,
    children: [
        { errorElement: <ErrorPage /> }
        , { element: <Home />, index: true }
        , { path: '/home', element: <Home /> }
        , { 
            path: '/pc', 
            children:[
                {path:'/pc/spu',index:true,element:<Product />,}
            ]
         }
    ]
},
{
    path:"/login",
    element: <Login />
}];
export { routes }
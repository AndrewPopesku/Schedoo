import ApiAuthorzationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import Schedule from "./pages/Schedule";

const AppRoutes = [
  {
    index: true,
    element: <Schedule />
  },
  // {
  //   path: '/counter',
  //   element: <Counter />
  // },
  // {
  //   path: '/fetch-data',
  //   requireAuth: true,
  //   element: <FetchData />
  // },
  ...ApiAuthorzationRoutes
];

export default AppRoutes;

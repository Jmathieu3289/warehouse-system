import { Routes, Route, Outlet, Link } from "react-router-dom";
import './App.css';
import { Navbar, Container, Nav, Button } from "react-bootstrap";
import { LinkContainer } from "react-router-bootstrap";

import Warehouse from "./components/warehouse";
import Items from "./components/items";
import PurchaseOrders from "./components/purchase-orders";
import SalesOrders from "./components/sales-orders";

function App() {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Home />} />
          <Route path="warehouse" element={<Warehouse />} />
          <Route path="items" element={<Items />} />
          <Route path="purchase-orders" element={<PurchaseOrders />} />
          <Route path="sales-orders" element={<SalesOrders />} />
          <Route path="*" element={<NoMatch />} />
        </Route>
      </Routes>
    </div>
  );
}

function Layout() {
  return (
    <div>
      <Navbar expand="lg" className="bg-body-tertiary border-bottom">
        <Container>
          <LinkContainer to="/">
            <Navbar.Brand>
              <img
                alt=""
                src="/logo128.png"
                width="30"
                height="30"
                className="d-inline-block align-top me-1"
              />{' '}
              WMS Client
            </Navbar.Brand>
          </LinkContainer>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Navbar.Collapse id="basic-navbar-nav">
            <Nav className="me-auto">
              <LinkContainer to="/">
                <Nav.Link>Home</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/warehouse">
                <Nav.Link>Warehouse</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/items">
                <Nav.Link>Items</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/purchase-orders">
                <Nav.Link>Purchase Orders</Nav.Link>
              </LinkContainer>
              <LinkContainer to="/sales-orders">
                <Nav.Link>Sales Orders</Nav.Link>
              </LinkContainer>
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
      <main className="mt-3">
        <Outlet />
      </main>
    </div>
  );
}

function Home() {
  return (
    <Container className="col-xxl-8 px-4 py-0 py-lg-5">
      <div className="row flex-lg-row-reverse align-items-center g-0 g-lg-5 py-0 py-lg-5 mt-0 mt-lg-3">
        <div className="col-10 col-sm-8 col-lg-6">
          <img src="/hero.jpg" className="d-block mx-auto img-fluid rounded" alt="Bootstrap Themes" loading="lazy" width="400" height="450" />
        </div>
        <div className="col-lg-6">
          <h1 className="display-5 fw-bold text-body-emphasis lh-1 mb-3">Welcome to my<br></br> WMS Client Project</h1>
          <p className="lead">Utilizing <code>React</code>, <code>React-Router</code>, <code>Bootstrap</code> and <code>React-Bootstrap</code> to get up and running quickly, 
          you can set up a warehouse, build items, and create purchase orders and sales orders from inventory. Leveraging an API built with <code>EF Core</code> and <code>ASP.NET</code>.</p>
          <div className="d-grid gap-2 d-md-flex justify-content-md-center">
            <LinkContainer to="/warehouse">
              <Button className="btn btn-primary btn-lg px-5 me-md-2">Get Started</Button>
            </LinkContainer>
          </div>
        </div>
      </div>
    </Container>
  );
}

function NoMatch() {
  return (
    <Container>
      <h2>Nothing to see here!</h2>
      <p>
        <Link to="/">Go to the home page</Link>
      </p>
    </Container>
  );
}

export default App;

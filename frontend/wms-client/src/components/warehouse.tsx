import { useState, useEffect } from "react";
import { Container, Button, Table, Modal, Row, Form } from "react-bootstrap";
import PalletBay from "../models/pallet-bay";

function Warehouse() {
  
    const [show, setShow] = useState(false);
    const [palletBays, setPalletBays] = useState<PalletBay[]>([]);
    const [loading, setLoading] = useState(true);
  
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    
    const fetchBays = async () => {
      setLoading(true);
      let res = await fetch('/api/palletbay', {
        method: 'GET'
      });
      let data = await res.json() as PalletBay[];
      setPalletBays(data);
      setLoading(false);
    }
  
    useEffect(() => {
      fetchBays();
    }, []);
  
    const handleSave = async (e: React.SyntheticEvent) => {
      e.preventDefault();
  
      const target = e.target as typeof e.target & {
        row: { value: string};
        startSection: { value: number };
        endSection: { value: number };
        startFloor: { value: number };
        endFloor: { value: number };
      }
  
      if (target && target.row.value.length > 0 && 
          target.startSection.value > 0 && target.endSection.value >= target.startSection.value && 
          target.startFloor.value > 0 && target.endFloor.value >= target.startFloor.value) {
  
        let data: any = {
          row: target.row.value,
          startSection: target.startSection.value,
          endSection: target.endSection.value,
          startFloor: target.startFloor.value,
          endFloor: target.endFloor.value
        };
  
        try {
          
          let res = await fetch('/api/palletbay/bulk', {
            method: 'POST',
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(data)
          });
    
          if (res.status === 200 || res.status === 204) {
            fetchBays();
          } else {
            //failure
          }
        } catch (err) {
          console.log(err);
        }
      }
  
      setShow(false);
    };
  
    return (
      <Container>
        <h2>Warehouse</h2>
        <hr className="w-25 mb-4"></hr>
        <div>
          <h4>Current Pallet Bays</h4>
          <div className="p-4 d-flex flex-column align-items-center">
                { (palletBays.length === 0) && !loading &&
                    <div className="text-center">
                      <p className="display-4 text-secondary">No Pallet Bays</p>
                      <Button variant="primary" onClick={handleShow}>Add Pallet Bays to Start</Button>
                    </div>
                }
                { (palletBays.length === 0) && loading &&
                    <div className="spinner"></div>
                }
                {
                  (palletBays.length > 0) &&
                    <Table size="sm" className="w-100" striped bordered>
                      <thead>
                        <tr>
                          <th className="col-1">Row</th>
                          <th className="col-1">Section</th>
                          <th className="col-1">Floor</th>
                          <th>Contents</th>
                          <th className="col-1"></th>
                        </tr>
                      </thead>
                      <tbody>
                        {
                          palletBays.map((bay: PalletBay) => {
                            return (
                              <tr key={bay.id}>
                                <td>{bay.row}</td>
                                <td>{bay.section}</td>
                                <td>{bay.floor}</td>
                                <td>{bay.pallets.filter(pallet => pallet.purchaseOrderItems.length > 0).map((pallet: any) => { 
                                  return <div>{pallet.purchaseOrderItems.map((poi: any) => {
                                    return <div>{poi.currentQuantity + ' ' + poi.item.name}</div>
                                  })}</div>
                                })}</td>
                                <td className="text-center"><Button variant="danger" size="sm" className="w-100">Remove</Button></td>
                              </tr>
                            )
                          })
                        }
                      </tbody>
                    </Table>
                }
          </div>
        </div>
        <div className="mt-4">
          <div className="mt-3">
            <Button variant="primary" type="button" className="px-4" onClick={handleShow}>
              Add Pallet Bays
            </Button>
          </div>
        </div>
        <Modal show={show} onHide={handleClose} animation={false} size="sm" centered>
          <Modal.Header closeButton>
            <Modal.Title>Add Pallet Bays</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form className="d-flex flex-column align-items-center" method="post" onSubmit={handleSave} autoComplete="off">
              <Form.Group className="mb-3 w-100" controlId="formRow">
                <Form.Label>Row</Form.Label>
                <Form.Control type="text" className="bg-dark-subtle w-100" name="row" autoFocus required />
                <Form.Text className="text-muted">
                  Typically one or two letters. E.g. "AA"
                </Form.Text>
              </Form.Group>
              <Row>
                <Form.Group className="mb-3 col-6" controlId="formEndSection">
                  <Form.Label>Start Section</Form.Label>
                  <Form.Control type="number" min="1" className="bg-dark-subtle" name="startSection" required />
                </Form.Group>
                <Form.Group className="mb-3 col-6" controlId="formEndSection">
                  <Form.Label>End Section</Form.Label>
                  <Form.Control type="number" min="1" className="bg-dark-subtle" name="endSection" required />
                </Form.Group>
              </Row>
              <Row>
                <Form.Group className="mb-3 col-6" controlId="formStartFloor">
                  <Form.Label>Start Floor</Form.Label>
                  <Form.Control type="number" min="1" className="bg-dark-subtle" name="startFloor" required />
                </Form.Group>
                <Form.Group className="mb-3 col-6" controlId="formEndFloor">
                  <Form.Label>End Floor</Form.Label>
                  <Form.Control type="number" min="1" className="bg-dark-subtle" name="endFloor" required />
                </Form.Group>
              </Row> 
              <div className="d-flex flex-row justify-content-end w-100">
                <Button variant="secondary" type="button" className="me-2" tabIndex={-1} onClick={handleClose}>Close</Button>
                <Button variant="success" type="submit">Save Pallet Bays</Button>
              </div>
            </Form>
          </Modal.Body>
        </Modal>
      </Container>
    );
}

export default Warehouse

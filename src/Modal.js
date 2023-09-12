import { Button, Box } from '@mui/material';

  const ModalBasket = ({ message, onContinueToBuy, onViewShoppingBasket }) => {
    return (
      <div className="modal"
      style={{
        position: 'fixed',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        backgroundColor: 'white',
        height: '40vh',
        width: '30vw',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
      }}
      >
        <div className="modal-content">
          <h3>{message}</h3>
          <Button onClick={onContinueToBuy}>Continue to Buy</Button>
          <Button onClick={onViewShoppingBasket}>Shopping Basket</Button>
        </div>
      </div>
    );
  };
  export default ModalBasket;
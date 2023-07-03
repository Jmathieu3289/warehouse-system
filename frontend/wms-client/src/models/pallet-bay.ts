import Contents from "./contents";

interface PalletBay {
    id: number;
    row: string;
    section: string;
    floor: string;
    contents: Contents[];
    pallets: any[];
}

export default PalletBay;
import DocumentsRow from "./documets-row"
import SendAllBtn from "./sendAll-btn";
import SignAllBtn from "./signAll-btn";

const documents= [
    {name: "Первый документ.pdf", signed: true},
    {name: "Второй документ.pdf", signed: false},
    {name: "Третий документ.pdf", signed: false},
]

export default function DocumentsTable () {
    return (
        <div>
            {documents.map((document) => {
                return (
                    <DocumentsRow
                        key={document.name}
                        signed={document.signed}
                        name={document.name}
                    />
                );
            })}
            <div className="flex items-center justify-end max-w-[800px]">
                <div className="mr-[15px]"><SignAllBtn /></div>
                <SendAllBtn />
            </div>
        </div>
    )
}